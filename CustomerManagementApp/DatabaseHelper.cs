using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace CustomerManagementApp
{
    class DatabaseHelper
    {
        private readonly string connectionString;

        public DatabaseHelper()
        {
            // Retrieve connection string from App.config
            connectionString = ConfigurationManager.ConnectionStrings["CustomerDB"].ConnectionString;
        }

        public List<Customer> GetCustomers()
        {
            var customers = new List<Customer>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Customers", conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(new Customer
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Age = reader.GetInt32(3),
                            Location = reader.GetString(4),
                            LastPurchaseDate = reader.GetDateTime(5),
                            LastUpdateDate = reader.GetDateTime(6),
                            PasswordHash = reader.IsDBNull(7) ? "" : reader.GetString(7),
                            Salt = reader.IsDBNull(8) ? "" : reader.GetString(8)
                        });
                    }
                }
            }
            return customers;
        }

        public void UpdateCustomers(List<Customer> customers)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                foreach (var customer in customers)
                {
                    using (SqlCommand cmd = new SqlCommand(
                        "UPDATE Customers SET LastName = @LastName, Age = @Age, Location = @Location, LastUpdateDate = @LastUpdateDate WHERE Id = @Id", conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", customer.Id);
                        cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                        cmd.Parameters.AddWithValue("@Age", customer.Age);
                        cmd.Parameters.AddWithValue("@Location", customer.Location);
                        cmd.Parameters.AddWithValue("@LastUpdateDate", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
