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
                LoggerHelper.Info("Customers loaded successfully.");
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("Error loading customers from the database.", ex);
                MessageBox.Show("Failed to load customers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                string filePath = PromptForInput("Enter file path for JSON export:");
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    FileHelper.ExportToJson(customers, filePath);
                    MessageBox.Show($"Data exported successfully to {filePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoggerHelper.Info($"Data exported successfully to {filePath}");
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
    }
}
