using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Khumalo_Craft_Emporium.Pages
{
    // PageModel class for AdminPage
    public class AdminPageModel : PageModel
    {
        // Connection string to the database (should be stored securely in configuration)
        public string connectionString = ""; // Update with your database connection string

        // List to hold orders
        public List<Order> Orders { get; set; } = new List<Order>();

        // List to hold products
        public List<Product> Products { get; set; } = new List<Product>();

        // OnGet method to fetch orders and products when the page is loaded
        public void OnGet()
        {
            FetchOrders();
            FetchProducts();
        }

        // Method to fetch orders from the database
        private void FetchOrders()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open(); // Open the database connection
                    string sql = "SELECT OrderId, UserId, InvoiceNumber, TotalPrice, OrderDate FROM [Order]";
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader(); // Execute the command and get a data reader

                    while (reader.Read())
                    {
                        // Add each order to the Orders list
                        Orders.Add(new Order
                        {
                            OrderId = reader.GetInt32(0),
                            UserId = reader.GetString(1),
                            InvoiceNumber = reader.GetString(2),
                            TotalPrice = reader.GetDecimal(3),
                            OrderDate = reader.GetDateTime(4)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching orders: " + ex.Message);
                // Handle exception
            }
        }

        // Method to fetch products from the database
        private void FetchProducts()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open(); // Open the database connection
                    string sql = "SELECT ProductId, Name, Price, Availability, Quantity FROM Product";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a new product and set its properties
                                Product products = new Product
                                {
                                    ProductId = Convert.ToInt32(reader["ProductId"]),
                                    Name = reader["Name"].ToString(),
                                    Price = Convert.ToDecimal(reader["Price"]),
                                    Availability = reader["Availability"].ToString(),
                                    Quantity = Convert.ToDecimal(reader["Quantity"])
                                };

                                // Add the product to the Products list
                                Products.Add(products);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching products: " + ex.Message);
                // Handle exception
            }
        }

        // Method to add a new product to the database
        public IActionResult OnPostAddProduct(string name, decimal price, int quantity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open(); // Open the database connection
                    string sql = "INSERT INTO Product (Name, Price, Quantity) VALUES (@Name, @Price, @Quantity)";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.ExecuteNonQuery(); // Execute the command
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding product: " + ex.Message);
                // Handle exception
            }

            return RedirectToPage(); // Redirect to the same page to refresh the data
        }
    }

    // Order class to represent an order
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }

    // Product class to represent a product
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Availability { get; set; }
        public decimal Quantity {  get; set; }
    }
}
