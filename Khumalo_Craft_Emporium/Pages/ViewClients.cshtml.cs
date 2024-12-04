using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Khumalo_Craft_Emporium.Pages.Clients
{
    public class ViewClientsModel : PageModel
    {
        public List<RegisterUser> listUsers  = new List<RegisterUser>();
        public string errorMessage = "";
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=st10356476.database.windows.net;Initial Catalog=st1035647;User ID=AdminSQL-st10356476;Password=Munyai@1";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM [User]";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RegisterUser user = new RegisterUser();
                                user.Username = reader.GetString(1);
                                user.Email = reader.GetString(2);
                               
                                // Password is not loaded here, as it should not be displayed on the registration page
                                listUsers.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "An error occurred while loading users: " + ex.Message;
                return;
            }
        }
    }

    public class RegisterUser
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
