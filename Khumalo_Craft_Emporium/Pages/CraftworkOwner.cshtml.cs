using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Khumalo_Craft_Emporium.Pages
{
    // PageModel class for uploading a new craftwork
    public class UploadCraftworkModel : PageModel
    {
        // Connection string to the database (should be stored securely in configuration)
        private readonly string _connectionString = "Data Source=st10356476.database.windows.net;Initial Catalog=st1035647;User ID=AdminSQL-st10356476;Password=Munyai@1";

        // HTTP POST method to handle the upload of a new craftwork
        [HttpPost]
        public IActionResult OnPost(string name, string description, decimal price, string category, string availability, IFormFile image)
        {
            // Generate a unique name for the uploaded image file
            string imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            // Define the path where the image will be saved
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", imageName);

            // Save the image to the specified path
            using (FileStream stream = new FileStream(imagePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            try
            {
                // Insert the new craftwork details into the database
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open(); // Open the database connection
                    // SQL query to insert the new craftwork
                    string sql = "INSERT INTO Product (Name, Description, Price, Category, Availability, ImagePath) VALUES (@Name, @Description, @Price, @Category, @Availability, @ImagePath);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Add parameters to the SQL query
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Category", category);
                        command.Parameters.AddWithValue("@Availability", availability);
                        command.Parameters.AddWithValue("@ImagePath", "/img/" + imageName);
                        command.ExecuteNonQuery(); // Execute the SQL query
                    }
                }

                // Redirect to the homepage or any other page after successful upload
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the upload process
                TempData["Error"] = "An error occurred while uploading the craftwork: " + ex.Message;
                return Page(); // Return to the current page to display the error message
            }
        }
    }
}
