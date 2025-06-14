using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SDAM_Assignment.Controllers
{
    class UserController
    {

        protected static string connectionString = "server=localhost;user=root;password=;database=marketplace;";

        public static List<User> LoadUsersByRole(string role)
        {
            List<User> users = new List<User>();

            using (var conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT id, name, email, phone_no, role FROM users WHERE role = @r";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@r", role);
                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string userRole = reader.GetString("role");

                        if (userRole == "Admin")
                        {
                            users.Add(new Admin
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                                Email = reader.GetString("email"),
                                Phone = reader.GetString("phone_no"),
                                Role = userRole
                            });
                        }
                        else if (userRole == "Seller")
                        {
                            users.Add(new Seller
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                                Email = reader.GetString("email"),
                                Phone = reader.GetString("phone_no"),
                                Role = userRole
                            });
                        }
                        else if (userRole == "Buyer")
                        {
                            users.Add(new Buyer
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                                Email = reader.GetString("email"),
                                Phone = reader.GetString("phone_no"),
                                Role = userRole
                            });
                        }
                        else
                        {
                            users.Add(new User
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("name"),
                                Email = reader.GetString("email"),
                                Phone = reader.GetString("phone_no"),
                                Role = userRole
                            });
                        }
                    }
                }
            }

            return users;
        }
    }
}
   

