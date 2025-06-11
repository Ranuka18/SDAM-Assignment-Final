using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SDAM_Assignment
{
<<<<<<< HEAD
    public partial class RegisterForm : Form
=======
    public partial class RegisterForm: Form
>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8
    {
        public RegisterForm()
        {
            InitializeComponent();
<<<<<<< HEAD
            cmbRole.Items.AddRange(new string[] { "admin", "seller", "buyer" });
=======
            cmbRole.Items.AddRange(new string[] {"admin", "seller", "buyer" });
>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8
            cmbRole.SelectedIndex = 0;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string role = cmbRole.SelectedItem.ToString().ToLower();
            string password = txtPassword.Text.Trim();
            string confirm = txtConfirm.Text.Trim();

<<<<<<< HEAD
            // ✅ Check for empty fields
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirm))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // ✅ Check if passwords match
=======
>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8
            if (password != confirm)
            {
                MessageBox.Show("Passwords do not match. Please try again.");
                return;
            }

            using (var conn = Database.GetConnection())
            {
<<<<<<< HEAD
                try
                {
                    conn.Open();

                    // ✅ Check if email already exists
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE email = @e";
                    using (var checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@e", email);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Email already registered. Please use a different one.");
                            return;
                        }
                    }

                    // ✅ Insert user
                    string insertQuery = "INSERT INTO users (name, email, phone_no, role, password) VALUES (@name, @email, @phone, @role, @password)";
                    using (var cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@role", role);
                        cmd.Parameters.AddWithValue("@password", password); 

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Account created successfully!");
                    this.Close(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}
=======
                string query = "INSERT INTO users (name, email, phone_no, role, password) VALUES (@name, @email, @phone, @role, @password)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@role", role);
                cmd.Parameters.AddWithValue("@password", password);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            MessageBox.Show("Account Created Succsessfully!");
            this.Close();  

        }
    }
}
>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8
