using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SDAM_Assignment
{
    public partial class LoginForm: Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = cmbRole.SelectedItem.ToString().ToLower();


            using (var conn = Database.GetConnection())
            {
                string query = "SELECT * FROM users WHERE email = @em AND password = @pw AND role = @role";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@em", email);
                cmd.Parameters.AddWithValue("@pw", password);
                cmd.Parameters.AddWithValue("@role", role);

                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int userId = Convert.ToInt32(reader["id"]);

                    if (role == "admin")
                    {
                        AdminDashboard adminForm = new AdminDashboard(userId); 
                        adminForm.Show();
                    }
                    else if (role == "seller")
                    {
                        SellerDashBoard sellerForm = new SellerDashBoard(userId);
                        sellerForm.Show();
                    }
                    else if (role == "buyer")
                    {                        
                        MessageBox.Show("Buyer login successful — route to BuyerDashboardForm.");                      
                    }

                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid login. Check email, password, and role.");
                }
            }
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            RegisterForm regForm = new RegisterForm();
            regForm.ShowDialog();
        }
    }
}
