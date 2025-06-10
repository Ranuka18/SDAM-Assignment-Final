using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SDAM_Assignment
{
    class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Phone { get; set; }

        private static string connectionString = "server=localhost;user=root;password=;database=marketplace;";

        public static List<User> LoadUsersByRole(string role)
        {
            List<User> users = new List<User>();

            using (var conn = Database.GetConnection())
            {
                string query = "SELECT id, name, email, phone_no FROM users WHERE role = @r";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@r", role);
                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Email = reader.GetString("email"),
                            Phone = reader.GetString("phone_no")
                        });
                    }
                }
            }

            return users;
        }

    }
}

