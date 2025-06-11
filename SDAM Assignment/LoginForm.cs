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
<<<<<<< HEAD
    public partial class LoginForm : Form
=======
    public partial class LoginForm: Form
>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8
    {
        public LoginForm()
        {
            InitializeComponent();
        }

<<<<<<< HEAD
        private void LoginForm_Load(object sender, EventArgs e)
        {
            cmbRole.Items.Add("Admin");
            cmbRole.Items.Add("Seller");
            cmbRole.Items.Add("Buyer");
            cmbRole.SelectedIndex = 0;
        }

=======
>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
<<<<<<< HEAD

            if (cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Please select a role.");
                return;
            }

            string role = cmbRole.SelectedItem.ToString().ToLower();

=======
            string role = cmbRole.SelectedItem.ToString().ToLower();


>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8
            using (var conn = Database.GetConnection())
            {
                string query = "SELECT * FROM users WHERE email = @em AND password = @pw AND role = @role";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@em", email);
                cmd.Parameters.AddWithValue("@pw", password);
                cmd.Parameters.AddWithValue("@role", role);

                conn.Open();
<<<<<<< HEAD
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
=======
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
>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8
                }
            }
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            RegisterForm regForm = new RegisterForm();
            regForm.ShowDialog();
        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8
