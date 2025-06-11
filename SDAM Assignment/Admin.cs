using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SDAM_Assignment
{
    public class Admin : User
    {
        public Admin() : base() { }

        public override void OpenDashboard()
        {
            AdminDashboard dashboard = new AdminDashboard(this);
            dashboard.Show();
        }

        public List<User> GetAllSellers()
        {
            return User.LoadUsersByRole("seller");
        }

        public bool DeleteSeller(int sellerId)
        {
            using (var conn = Database.GetConnection())
            {
                string query = "DELETE FROM users WHERE id = @id AND role = 'seller'";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", sellerId);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public Panel GenerateSellerCard(User seller, Action refreshCallback)
        {
            Panel card = new Panel
            {
                Width = 420,
                Height = 110,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10),
                BackColor = System.Drawing.Color.WhiteSmoke
            };

            Label lblInfo = new Label
            {
                Text = $"ID: {seller.Id}\nName: {seller.Name}\nEmail: {seller.Email}\nPhone: {seller.Phone}",
                Location = new System.Drawing.Point(10, 10),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };

            Button btnView = new Button
            {
                Text = "View Details",
                Width = 100,
                Height = 30,
                Location = new System.Drawing.Point(300, 10),
                BackColor = System.Drawing.Color.SteelBlue,
                ForeColor = System.Drawing.Color.White
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
                Location = new System.Drawing.Point(300, 50),
                BackColor = System.Drawing.Color.IndianRed,
                ForeColor = System.Drawing.Color.White
            };
            btnDelete.Click += (s, e) =>
            {
                DialogResult result = MessageBox.Show($"Are you sure you want to delete seller {seller.Name}?", "Confirm", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes && DeleteSeller(seller.Id))
                {
                    MessageBox.Show("Seller deleted successfully.");
                    refreshCallback?.Invoke();
                }
                else
                {
                    MessageBox.Show("Failed to delete seller.");
                }
            };

            card.Controls.Add(lblInfo);
            card.Controls.Add(btnView);
            card.Controls.Add(btnDelete);

            return card;
        }
    }
}
