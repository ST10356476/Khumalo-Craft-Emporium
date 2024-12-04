using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Khumalo_Craft_Emporium.Pages.Clients
{
    // PageModel class for user login
    public class LoginModel : PageModel
    {
        // Connection string to the database (should be stored securely in configuration)
        private readonly string _connectionString = ""; // Update with your database connection string

        // Property to bind the user's email
        [BindProperty]
        public string Email { get; set; }

        // Property to bind the user's password
        [BindProperty]
        public string Password { get; set; }

        // Property to hold error message if login fails
        public string ErrorMessage { get; set; }

        // HTTP POST method to handle user login
        public IActionResult OnPost()
        {
            // Validate the user's credentials
            var username = ValidateUser(Email, Password);
            if (!string.IsNullOrEmpty(username))
            {
                // Check if the user is an admin or a regular user
                if (username.Contains("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToPage("/AdminPage"); // Redirect to the admin page
                }
                else
                {
                    return RedirectToPage("/Checkout", new { totalPrice = 0, cartItems = new List<CartItem>() }); // Redirect to the checkout page for regular users
                }
            }
            else
            {
                ErrorMessage = "Invalid email or password"; // Display error message if login fails
                return Page(); // Stay on the login page
            }
        }

        // Method to validate user credentials against the database
        private string ValidateUser(string email, string password)
        {
            try
            {
                // Connect to the database
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open(); // Open the database connection

                    // SQL query to validate user credentials
                    string sql = "SELECT Username FROM [User] WHERE Email = @Email AND Password = @Password";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@Email", email); // Add email parameter
                    command.Parameters.AddWithValue("@Password", password); // Add password parameter

                    // Execute the SQL query and get the result
                    var result = command.ExecuteScalar();

                    // Return the username if the user exists, otherwise return null
                    return result != null ? result.ToString() : null;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the validation process
                Console.WriteLine("Error validating user: " + ex.Message);
                return null; // Return null in case of an exception
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
