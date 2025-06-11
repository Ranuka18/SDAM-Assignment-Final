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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            cmbRole.Items.Add("Admin");
            cmbRole.Items.Add("Seller");
            cmbRole.Items.Add("Buyer");
            cmbRole.SelectedIndex = 0;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Please select a role.");
                return;
            }

            string role = cmbRole.SelectedItem.ToString().ToLower();

            using (var conn = Database.GetConnection())
            {
                string query = "SELECT * FROM users WHERE email = @em AND password = @pw AND role = @role";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@em", email);
                cmd.Parameters.AddWithValue("@pw", password);
                cmd.Parameters.AddWithValue("@role", role);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Creating user based on role
                        User user;

                        switch (role)
                        {
                            case "admin":
                                user = new Admin();
                                break;
                            case "seller":
                                user = new Seller();
                                break;
                            case "buyer":
                                user = new Buyer();
                                break;
                            default:
                                MessageBox.Show("Unknown role.");
                                return;
                        }

                        // Assigning users properties from the database
                        user.Id = Convert.ToInt32(reader["id"]);
                        user.Name = reader["name"].ToString();
                        user.Email = reader["email"].ToString();
                        user.Phone = reader["phone_no"].ToString();
                        user.Role = role;

                        user.OpenDashboard();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid login. Check email, password, and role.");
                    }
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