using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Khumalo_Craft_Emporium.Pages.Clients
{
    public class RegisterModel : PageModel
    {
        private string connectionString = "Your_connection_string";

     
        public RegisterUser user = new RegisterUser();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            // Load users only if needed
        }

        public IActionResult OnPost()
        {
            user.Username = Request.Form["Username"];
            user.Email = Request.Form["Email"];
            user.Password = Request.Form["Password"];

            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
            {
                errorMessage = "Please fill in all fields";
                return Page();
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO [User] (Username, Email, Password) VALUES (@Username, @Email, @Password);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Username", user.Username);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@Password", user.Password); // Hash the password here
                        command.ExecuteNonQuery();
                    }
                }
                successMessage = "User registered successfully";
                return RedirectToPage("Register"); // Post-Redirect-Get pattern
            }
            catch (Exception ex)
            {
                errorMessage = "An error occurred: " + ex.Message;
                return Page();
            }

            
        }
    }
}
