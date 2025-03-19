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
        private Dictionary<(int rowIndex, string columnName), object> originalValues = new Dictionary<(int, string), object>();

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

                ConfigureGridView();

                // Set the header text for better readability
                customerGridView.Columns["FirstName"].HeaderText = "First Name";
                customerGridView.Columns["LastName"].HeaderText = "Last Name";
                customerGridView.Columns["LastPurchaseDate"].HeaderText = "Last Purchase Date";
                customerGridView.Columns["LastUpdateDate"].HeaderText = "Last Update Date";
                customerGridView.Columns["PasswordHash"].HeaderText = "Password Hash";

                // Make read-only columns visibly distinct
                customerGridView.Columns["Id"].ReadOnly = true;
                customerGridView.Columns["Id"].DefaultCellStyle.BackColor = Color.LightGray;
                customerGridView.Columns["Id"].DefaultCellStyle.ForeColor = Color.DarkGray;

                customerGridView.Columns["LastPurchaseDate"].ReadOnly = true;
                customerGridView.Columns["LastPurchaseDate"].DefaultCellStyle.ForeColor = Color.DarkGray;

                customerGridView.Columns["LastUpdateDate"].ReadOnly = true;
                customerGridView.Columns["LastUpdateDate"].DefaultCellStyle.ForeColor = Color.DarkGray;

                customerGridView.Columns["PasswordHash"].ReadOnly = true;
                customerGridView.Columns["PasswordHash"].DefaultCellStyle.ForeColor = Color.DarkGray;

                customerGridView.Columns["Salt"].ReadOnly = true;
                customerGridView.Columns["Salt"].DefaultCellStyle.ForeColor = Color.DarkGray;

                // Set alternating row colors
                customerGridView.RowsDefaultCellStyle.BackColor = Color.White;
                customerGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;


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

        private void ConfigureGridView()
        {
            var headers = new Dictionary<string, string>
            {
                ["FirstName"] = "First Name",
                ["LastName"] = "Last Name",
                ["LastPurchaseDate"] = "Last Purchase Date",
                ["LastUpdateDate"] = "Last Update Date",
                ["PasswordHash"] = "Password Hash",
                ["Salt"] = "Salt Value"
            };

            foreach (var column in customerGridView.Columns.Cast<DataGridViewColumn>())
            {
                if (headers.ContainsKey(column.Name))
                    column.HeaderText = headers[column.Name];

                if (IsReadOnlyColumn(column.Name))
                {
                    column.ReadOnly = true;
                    column.DefaultCellStyle.BackColor = Color.LightGray;
                    column.DefaultCellStyle.ForeColor = Color.DarkGray;
                }
            }

            customerGridView.RowsDefaultCellStyle.BackColor = Color.White;
            customerGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;
        }

        private static bool IsReadOnlyColumn(string columnName)
        {
            return columnName == "Id" || columnName == "LastPurchaseDate" || columnName == "LastUpdateDate" || columnName == "PasswordHash" || columnName == "Salt";
        }


        private void customerGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var row = customerGridView.Rows[e.RowIndex];
                var cell = row.Cells[e.ColumnIndex];
                var customer = row.DataBoundItem as Customer;

                if (customer != null)
                {
                    string propertyName = customerGridView.Columns[e.ColumnIndex].DataPropertyName;
                    var originalCustomer = customers.FirstOrDefault(c => c.Id == customer.Id);
                    if (originalCustomer != null)
                    {
                        var originalValue = originalCustomer.GetType().GetProperty(propertyName)?.GetValue(originalCustomer, null);
                        var key = (e.RowIndex, propertyName);

                        // Store the original value if it's not already stored
                        if (!originalValues.ContainsKey(key))
                        {
                            originalValues[key] = originalValue;
                        }
                    }
                }
            }
        }

        private void customerGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var row = customerGridView.Rows[e.RowIndex];                
                var customer = row.DataBoundItem as Customer;

                if (customer != null)
                {
                    string propertyName = customerGridView.Columns[e.ColumnIndex].DataPropertyName;
                    var key = (e.RowIndex, propertyName);

                    if (originalValues.TryGetValue(key, out var originalValue))
                    {
                        var currentValue = customer.GetType().GetProperty(propertyName)?.GetValue(customer);
                        bool isModified = originalValue?.ToString() != currentValue?.ToString();

                        // Value reverted to original, set color back to black
                        row.Cells[e.ColumnIndex].Style.ForeColor = isModified ? Color.Red : Color.Black;

                        if (isModified)
                            modifiedCustomers.Add(customer);
                        else
                            modifiedCustomers.Remove(customer);
                    }
                }
            }
        }

        private void customerGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (customerGridView.Columns[e.ColumnIndex].Name == "Age")
            {
                string input = e.FormattedValue.ToString();

                if (!int.TryParse(input, out int age) || age <= 0)
                {
                    MessageBox.Show("Invalid input! Please enter a numeric value for Age.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // Prevents leaving the cell until a valid value is entered
                    e.Cancel = true;
                }
            }
            // Check if the column being edited is FirstName, LastName, or Location
            else if (customerGridView.Columns[e.ColumnIndex].Name == "FirstName" ||
                     customerGridView.Columns[e.ColumnIndex].Name == "LastName" ||
                     customerGridView.Columns[e.ColumnIndex].Name == "Location")
            {
                string input = e.FormattedValue.ToString();

                if (string.IsNullOrEmpty(input))
                {
                    string fieldName = customerGridView.Columns[e.ColumnIndex].HeaderText;
                    MessageBox.Show($"{fieldName} is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // Prevents leaving the cell until a value is entered
                    e.Cancel = true; 
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
            originalValues.Clear();
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
                    string filePath = PromptForValidFilePath();
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        FileHelper.ExportToJson(filteredCustomers, filePath);
                        MessageBox.Show($"Data exported successfully to {filePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoggerHelper.Info($"Data exported successfully to {filePath}");
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

        private string PromptForValidFilePath()
        {
            while (true)
            {
                string filePath = PromptForInput("Enter the full file path for JSON export (e.g., C:\\CATALIS\\customer.json):");

                // Ensure the file path is not empty, is an absolute path, has a valid filename, and ends with .json or .txt
                if (!string.IsNullOrWhiteSpace(filePath) &&
                    // Ensure the path is absolute (contains directory path)
                    Path.IsPathRooted(filePath) &&
                    // Ensure there is a filename (after the last separator)
                    !string.IsNullOrWhiteSpace(Path.GetFileName(filePath)) &&
                    // Ensure there is something before the extension
                    !string.IsNullOrWhiteSpace(Path.GetFileNameWithoutExtension(filePath)) &&
                    // Ensure correct extension
                    (filePath.EndsWith(".json") || filePath.EndsWith(".txt")))
                {
                    return filePath;
                }

                MessageBox.Show("Invalid file path. Ensure it includes a valid filename with .json or .txt extension.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
