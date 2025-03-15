using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CustomerManagementApp.Utils;
using Microsoft.VisualBasic;

namespace CustomerManagementApp
{
    public partial class MainForm : Form
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();
        private List<Customer> customers;

        public MainForm()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                customers = dbHelper.GetCustomers();
                customerGridView.DataSource = customers;

                // Allow editing of First Name, Last Name and Age
                customerGridView.Columns["FirstName"].ReadOnly = false;  
                customerGridView.Columns["LastName"].ReadOnly = false;
                customerGridView.Columns["Age"].ReadOnly = false;
                customerGridView.Columns["Location"].ReadOnly = false;


                // Check if password is blank, and call SetPassword for each customer with blank password
                foreach (var customer in customers)
                {
                    // Check if the password is blank
                    if (string.IsNullOrEmpty(customer.PasswordHash)) 
                    {
                        // Set the password and salt
                        SetPassword(customer);
                        LoggerHelper.Info($"Password set for customer: {customer.FirstName} {customer.LastName}");
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

        public void SetPassword(Customer customer)
        {
            // Generate a random salt of length 16
            string salt = PasswordHelper.GenerateSalt();

            // Compute the SHA1 hash of "password" concatenated with the salt
            string passwordHash = PasswordHelper.GeneratePasswordHash(salt);

            // Update the customer's PasswordHash and Salt
            customer.PasswordHash = passwordHash;
            customer.Salt = salt;

            // Save the updated customer to the database
            dbHelper.UpdateCustomers(new List<Customer> { customer });

            LoggerHelper.Info($"Password set and updated for customer: {customer.FirstName} {customer.LastName}");
        }

        private void btnUppercaseLastNames_Click(object sender, EventArgs e)
        {
            foreach (var customer in customers)
            {
                customer.LastName = customer.LastName.ToUpper();
            }
            customerGridView.Refresh();
        }

        private void btnCommitChanges_Click(object sender, EventArgs e)
        {
            dbHelper.UpdateCustomers(customers);
            LoadCustomers();
            MessageBox.Show("Changes committed to the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExportJson_Click(object sender, EventArgs e)
        {
            try
            {
                // Prompt for first name to filter customers
                string firstName = PromptForInput("Enter the first name to search for:");

                // Filter customers by first name (case-insensitive)
                var filteredCustomers = customers.Where(c => c.FirstName != null && c.FirstName.ToLower().Contains(firstName.ToLower())).ToList();

                if (filteredCustomers.Any())
                {
                    // Prompt for the file path to save the JSON export
                    string filePath = PromptForInput("Enter the directory path for JSON export:");

                    if (!string.IsNullOrWhiteSpace(filePath))
                    {
                        // Use FileHelper to export the filtered customers to the provided file path
                        FileHelper.ExportToJson(filteredCustomers, filePath);

                        MessageBox.Show($"Data exported successfully to {filePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoggerHelper.Info($"Data exported successfully to {filePath}");
                    }
                    else
                    {
                        MessageBox.Show("Invalid file path. Please provide a valid path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                var filteredCustomers = customers.Where(c => c.Age <= maxAge).ToList();
                customerGridView.DataSource = filteredCustomers;
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
