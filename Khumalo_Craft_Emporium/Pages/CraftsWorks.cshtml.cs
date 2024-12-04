using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Khumalo_Craft_Emporium.Pages
{
    // PageModel class for CraftWorks
    public class CraftsWorksModel : PageModel
    {
        // List to hold the craftwork items
        public List<Craftwork> Craftworks { get; set; } = new List<Craftwork>();

        // OnGet method to fetch the craftworks when the page is loaded
        public void OnGet()
        {
            FetchCraftworks(); // Fetch the craftworks from the database
        }

        // Method to fetch the craftworks from the database
        private void FetchCraftworks()
        {
            try
            {
                // Database connection string (should be stored securely in configuration)
                string connectionString = "Your Connection string";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open(); // Open the database connection

                    // SQL query to fetch craftwork details
                    string sql = "SELECT ProductId, Name, Description, Price, Category, Availability, ImagePath FROM Product";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader()) // Execute the command and get a data reader
                        {
                            while (reader.Read())
                            {
                                // Create a new craftwork item and set its properties
                                Craftwork craftwork = new Craftwork
                                {
                                    Id = Convert.ToInt32(reader["ProductId"]),
                                    Name = reader["Name"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Price = Convert.ToDecimal(reader["Price"]),
                                    Category = reader["Category"].ToString(),
                                    Availability = reader["Availability"].ToString(),
                                    ImagePath = reader["ImagePath"].ToString()
                                };

                                // Add the craftwork item to the Craftworks list
                                Craftworks.Add(craftwork);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and log the error message
                Console.WriteLine("An error occurred while fetching craftworks: " + ex.Message);
            }
        }
    }

    // Class to represent a craftwork item
    public class Craftwork
    {
        public int Id { get; set; } // Craftwork ID
        public string Name { get; set; } // Craftwork name
        public string Description { get; set; } // Craftwork description
        public decimal Price { get; set; } // Craftwork price
        public string Category { get; set; } // Craftwork category
        public string Availability { get; set; } // Craftwork availability status
        public string ImagePath { get; set; } // Path to the craftwork image
    }
}
