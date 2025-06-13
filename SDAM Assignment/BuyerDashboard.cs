using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDAM_Assignment.Helpers;

namespace SDAM_Assignment
{
    public partial class BuyerDashboard: Form
    {
        private Buyer buyer;

        public BuyerDashboard(Buyer buyer)
        {
            InitializeComponent();
            FormStyler.ApplyTheme(this);
            this.buyer = buyer;
        }


        private void btnShop_Click(object sender, EventArgs e)
        {
            BrowseProductsForm browseProductsForm = new BrowseProductsForm(this.buyer);
            browseProductsForm.Show();
        }

        private void btCart_Click(object sender, EventArgs e)
        {
            CartForm cartForm = new CartForm(buyer);
            cartForm.ShowDialog();
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            BuyerOrdersForm ordersForm = new BuyerOrdersForm(this.buyer);
            ordersForm.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            {
                DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    this.Hide();
                    LoginForm login = new LoginForm();
                    login.Show();
                }
            }
        }

        private void BuyerDashboard_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = "Welcome, " + buyer.Name + "!";
            lblName.Text = "Name : " + buyer.Name;
            lblEmail.Text = "Email : " + buyer.Email;
            iblPhone.Text = "Phone No : " + buyer.Phone;

        }
    }
}