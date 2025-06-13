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
using SDAM_Assignment.Helpers;

namespace SDAM_Assignment
{
    public partial class SellerDashBoard : Form
    {
        private int sellerId;
        private Seller seller;

        public SellerDashBoard(int sellerId)
        {
            InitializeComponent();
            FormStyler.ApplyTheme(this);
            this.sellerId = sellerId;
            this.seller = Seller.GetSellerById(sellerId);

            if (seller != null)
            {
                lblWelcome.Text = $"Welcome {seller.Name}";
            }
           
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            AddProductForm form = new AddProductForm(sellerId);
            form.ShowDialog();
        }

        private void btnViewProducts_Click(object sender, EventArgs e)
        {
            ViewProductsForm form = new ViewProductsForm(seller);
            form.Show();
        }

        private void btnViewOrders_Click(object sender, EventArgs e)
        {
            SellerOrdersForm ordersForm = new SellerOrdersForm(this.seller);
            ordersForm.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
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
}