using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SDAM_Assignment
{
<<<<<<< HEAD
    public partial class ViewSellers : Form
    {
        private Admin currentAdmin;

        public ViewSellers(Admin admin)
        {
            InitializeComponent();
            currentAdmin = admin;
=======
    public partial class ViewSellers: Form
    {
        public ViewSellers()
        {
            InitializeComponent();
>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8
            LoadSellers();
        }

        private void LoadSellers()
        {
            flowLayoutPanelSellers.Controls.Clear();

<<<<<<< HEAD
            List<User> sellers = currentAdmin.GetAllSellers();

            foreach (var seller in sellers)
            {
                Panel card = currentAdmin.GenerateSellerCard(seller, LoadSellers);
                flowLayoutPanelSellers.Controls.Add(card);
            }
        }
    }
}

=======
            List<User> sellers = User.LoadUsersByRole("seller");

            foreach (var seller in sellers)
            {
                Panel card = CreateSellerCard(seller);
                flowLayoutPanelSellers.Controls.Add(card);
            }
        }

        private Panel CreateSellerCard(User seller)
        {
            Panel card = new Panel
            {
                Width = 420,
                Height = 110,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10),
                BackColor = Color.WhiteSmoke
            };

            Label lblInfo = new Label
            {
                Text = $"ID: {seller.Id}\nName: {seller.Name}\nEmail: {seller.Email}\nPhone: {seller.Phone}",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };

            Button btnView = new Button
            {
                Text = "View Details",
                Width = 100,
                Height = 30,
                Location = new Point(300, 10),
                BackColor = Color.SteelBlue,
                ForeColor = Color.White
            };

            btnView.Click += (s, e) =>
            {
                SellerDetailsForm form = new SellerDetailsForm(seller.Id);
                form.ShowDialog();
            };

            Button btnDelete = new Button
            {
                Text = "Delete",
                Width = 100,
                Height = 30,
                Location = new Point(300, 50),
                BackColor = Color.IndianRed,
                ForeColor = Color.White
            };

            btnDelete.Click += (s, e) =>
            {
                DialogResult result = MessageBox.Show($"Are you sure you want to delete seller {seller.Name}?", "Confirm", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    DeleteSeller(seller.Id);
                    LoadSellers();
                }
            };

            card.Controls.Add(lblInfo);
            card.Controls.Add(btnView);
            card.Controls.Add(btnDelete);

            return card;
        }

        private void DeleteSeller(int sellerId)
        {
            using (var conn = Database.GetConnection())
            {
                string query = "DELETE FROM users WHERE id = @id AND role = 'seller'";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", sellerId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Seller deleted successfully.");
        }
    }
}


>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8
