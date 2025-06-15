using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SDAM_Assignment
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Phone { get; set; }

        protected static string connectionString = "server=localhost;user=root;password=;database=marketplace;";

        public User() { }

        public virtual void OpenDashboard()
        {
            MessageBox.Show("Opening default user dashboard...");
        }
    }
}

