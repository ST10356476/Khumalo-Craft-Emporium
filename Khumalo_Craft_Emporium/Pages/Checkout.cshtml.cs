using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Khumalo_Craft_Emporium.Pages
{
    // PageModel class for Checkout
    public class CheckoutModel : PageModel
    {
        // Connection string to the database (should be stored securely in configuration)
        private readonly string _connectionString = ""; // Update with your database connection string

        // Property to bind the total price
        [BindProperty]
        public decimal TotalPrice { get; set; }

        // Property to bind the list of cart items
        [BindProperty]
        public List<CartItem> CartItems { get; set; }

        // OnGet method to initialize the page with total price and cart items
        public void OnGet(decimal totalPrice, List<CartItem> cartItems)
        {
            TotalPrice = totalPrice; // Assign total price
            CartItems = cartItems; // Assign cart items
        }

        // OnPost method to handle the download invoice action
        public IActionResult OnPostDownloadInvoice()
        {
            string userId = User.Identity.Name; // Get the user ID from authentication
            string email = "user@example.com"; // Get the user's email from authentication (placeholder)
            string invoiceNumber = GenerateInvoiceNumber(); // Generate an invoice number
            StoreOrderInDatabase(userId, TotalPrice, invoiceNumber); // Store the order in the database
            TempData["InvoiceNumber"] = invoiceNumber; // Store the invoice number in TempData
            return RedirectToPage("/OrderConfirmation"); // Redirect to OrderConfirmation page
        }

        // Method to generate a unique invoice number
        private string GenerateInvoiceNumber()
        {
            return $"INV-{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        // Method to store the order in the database
        private void StoreOrderInDatabase(string userId, decimal totalPrice, string invoiceNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open(); // Open the database connection
                    string sql = "INSERT INTO [Order] (UserId, OrderDate, InvoiceNumber, TotalPrice) " +
                                 "VALUES (@UserId, @OrderDate, @InvoiceNumber, @TotalPrice)";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@UserId", userId); // Add user ID parameter
                    command.Parameters.AddWithValue("@OrderDate", DateTime.Now); // Add order date parameter
                    command.Parameters.AddWithValue("@InvoiceNumber", invoiceNumber); // Add invoice number parameter
                    command.Parameters.AddWithValue("@TotalPrice", totalPrice); // Add total price parameter
                    command.ExecuteNonQuery(); // Execute the command
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Log the exception or handle it appropriately
            }
        }
    }

    // Class to represent an item in the shopping cart
    public class CartItem
    {
        public string Name { get; set; } // Name of the item
        public int Quantity { get; set; } // Quantity of the item
        public decimal Price { get; set; } // Price of the item
    }
}
