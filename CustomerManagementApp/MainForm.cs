using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CustomerManagementApp.Utils;
using Microsoft.VisualBasic;

namespace CustomerManagementApp
{
    public partial class MainForm : Form
    {
        private List<Customer> customers;
        private List<Customer> modifiedCustomers = new List<Customer>();


        public MainForm()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                using (var context = new CustomerContext())
                {
                    customers = context.Customers.ToList();
                    customerGridView.DataSource = customers;
                }

                customerGridView.Columns["FirstName"].ReadOnly = false;
                customerGridView.Columns["LastName"].ReadOnly = false;
                customerGridView.Columns["Age"].ReadOnly = false;
                customerGridView.Columns["Location"].ReadOnly = false;

                foreach (var customer in customers)
                {
                    if (string.IsNullOrEmpty(customer.PasswordHash))
                    {
                        SetPassword(customer);
                    }
                }
                LoggerHelper.Info("Customers loaded successfully.");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("Error loading customers from the database.", ex);
                MessageBox.Show("Failed to load customers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void customerGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure it's a valid row
            if (e.RowIndex >= 0) 
            {
                var customer = customerGridView.Rows[e.RowIndex].DataBoundItem as Customer;
                if (customer != null && !modifiedCustomers.Contains(customer))
                {
                    modifiedCustomers.Add(customer);

                    customerGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
                }
            }
        }


        public void SetPassword(Customer customer)
        {
            string salt = PasswordHelper.GenerateSalt();
            string passwordHash = PasswordHelper.GeneratePasswordHash(salt);

            customer.PasswordHash = passwordHash;
            customer.Salt = salt;

            using (var context = new CustomerContext())
            {
                context.Entry(customer).State = EntityState.Modified;
                context.SaveChanges();
            }
            LoggerHelper.Info($"Password set and updated for customer: {customer.FirstName} {customer.LastName}");
        }

        private void btnUppercaseLastNames_Click(object sender, EventArgs e)
        {
            foreach (var customer in customers)
            {
                // Find the customer row in the DataGridView by matching the customer object
                var rowIndex = customerGridView.Rows.Cast<DataGridViewRow>()
                    .FirstOrDefault(r => r.DataBoundItem == customer)?.Index;

                if (rowIndex.HasValue)
                {
                    // Check if the LastName has been changed and if it's not already in the modifiedCustomers list
                    if (customer.LastName != customer.LastName.ToUpper())
                    {
                        // Update LastName to uppercase
                        customer.LastName = customer.LastName.ToUpper();

                        // Add the modified customer to the modifiedCustomers list
                        if (!modifiedCustomers.Contains(customer))
                        {
                            modifiedCustomers.Add(customer);
                        }

                        // Change the text color to red for the updated LastName column cell
                        customerGridView.Rows[rowIndex.Value].Cells["LastName"].Style.ForeColor = Color.Red;
                    }
                }
            }

            // Refresh the grid to reflect changes
            customerGridView.Refresh();
        }


        private void btnCommitChanges_Click(object sender, EventArgs e)
        {
            if (modifiedCustomers.Count == 0)
            {
                MessageBox.Show("No changes detected.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var context = new CustomerContext())
            {
                foreach (var customer in modifiedCustomers)
                {
                    var existingCustomer = context.Customers.Find(customer.Id);
                    if (existingCustomer != null)
                    {
                        // Update fields if changed
                        existingCustomer.FirstName = customer.FirstName;
                        existingCustomer.LastName = customer.LastName;
                        existingCustomer.Age = customer.Age;
                        existingCustomer.Location = customer.Location;

                        // Update LastUpdateDate
                        existingCustomer.LastUpdateDate = DateTime.Now;
                    }
                }
                context.SaveChanges();
            }

            // Reset the list after committing changes
            modifiedCustomers.Clear();

            // Reset the font color to black for all cells in the DataGridView
            customerGridView.DefaultCellStyle.ForeColor = Color.Black;

            // Refresh data
            LoadCustomers(); 
            MessageBox.Show("Changes committed to the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExportJson_Click(object sender, EventArgs e)
        {
            try
            {
                string firstName = PromptForInput("Enter the first name to search for:");
                var filteredCustomers = customers.Where(c => c.FirstName != null && c.FirstName.ToLower().Contains(firstName.ToLower())).ToList();

                if (filteredCustomers.Any())
                {
                    string filePath;
                    bool isValidPath = false;

                    // Loop until a valid file path is entered
                    while (!isValidPath)
                    {
                        filePath = PromptForInput("Enter the full file path for JSON export (including filename, e.g., C:\\CATALIS\\customer.json):");

                        if (!string.IsNullOrWhiteSpace(filePath))
                        {
                            // Check if the file path includes a valid filename
                            string fileName = Path.GetFileName(filePath);

                            // Get the file extension
                            string fileExtension = Path.GetExtension(filePath).ToLower();

                            if (string.IsNullOrEmpty(fileName) || !fileExtension.Equals(".json") && !fileExtension.Equals(".txt"))
                            {
                                MessageBox.Show("The file path must include a filename with an extension. Please provide a valid path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                // Valid path with filename, exit loop
                                isValidPath = true; 
                                FileHelper.ExportToJson(filteredCustomers, filePath);
                                MessageBox.Show($"Data exported successfully to {filePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoggerHelper.Info($"Data exported successfully to {filePath}");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid file path. Please provide a valid path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No customers found matching the first name.", "No Matches", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("Error exporting data to JSON.", ex);
                MessageBox.Show("Failed to export data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnSearchByAge_Click(object sender, EventArgs e)
        {
            string input = PromptForInput("Enter maximum age:");
            if (int.TryParse(input, out int maxAge))
            {
                using (var context = new CustomerContext())
                {
                    var filteredCustomers = context.Customers.Where(c => c.Age <= maxAge).ToList();
                    customerGridView.DataSource = filteredCustomers;
                }
            }
            else
            {
                MessageBox.Show("Invalid age entered. Please enter a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string PromptForInput(string message)
        {
            return Interaction.InputBox(message, "Input Required", "");
        }

        private void btnRefreshGrid_Click(object sender, EventArgs e)
        {
            LoadCustomers();
        }
    }
}
