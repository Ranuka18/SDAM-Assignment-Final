using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
using System.Windows.Forms;
=======
>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8
using MySql.Data.MySqlClient;

namespace SDAM_Assignment
{
<<<<<<< HEAD
    public class User
=======
    class User
>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Phone { get; set; }

<<<<<<< HEAD
        protected static string connectionString = "server=localhost;user=root;password=;database=marketplace;";

        public User() { }

        public virtual void OpenDashboard()
        {
            MessageBox.Show("Opening default user dashboard...");
        }
=======
        private static string connectionString = "server=localhost;user=root;password=;database=marketplace;";
>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8

        public static List<User> LoadUsersByRole(string role)
        {
            List<User> users = new List<User>();

<<<<<<< HEAD
            using (var conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT id, name, email, phone_no, role FROM users WHERE role = @r";
=======
            using (var conn = Database.GetConnection())
            {
                string query = "SELECT id, name, email, phone_no FROM users WHERE role = @r";
>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@r", role);
                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
<<<<<<< HEAD
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
=======
                        users.Add(new User
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Email = reader.GetString("email"),
                            Phone = reader.GetString("phone_no")
                        });
>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8
                    }
                }
            }

            return users;
        }
<<<<<<< HEAD
=======

>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8
    }
}

