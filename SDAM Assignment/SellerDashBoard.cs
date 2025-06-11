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
    public partial class SellerDashBoard : Form
    {
        private int sellerId;
        private Seller seller;

        public SellerDashBoard(int sellerId)
        {
            InitializeComponent();
            this.sellerId = sellerId;
            this.seller = Seller.GetSellerById(sellerId);

            if (seller != null)
            {
                lblWelcome.Text = $"Welcome {seller.Name}";
            }
           
        }
=======
        public partial class SellerDashBoard : Form
        {
            private int sellerId;

            public SellerDashBoard(int sellerId)
            {
                InitializeComponent();
                this.sellerId = sellerId;
            }

            private void SellerDashBoard_Load(object sender, EventArgs e)
            {
>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8

            }

<<<<<<< HEAD
        private void btnViewProducts_Click(object sender, EventArgs e)
        {
            ViewProductsForm form = new ViewProductsForm(seller);
            form.Show();
        }
=======
            private void btnAddProduct_Click(object sender, EventArgs e)
            {
                AddProductForm form = new AddProductForm(sellerId);
                form.ShowDialog();
            }
>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8

            private void btnViewProducts_Click(object sender, EventArgs e)
            {
                ViewProductsForm form = new ViewProductsForm();
                form.Show();
            }

            private void btnViewOrders_Click(object sender, EventArgs e)
            {
                ViewOrdersForm form = new ViewOrdersForm(sellerId);
                form.ShowDialog();
            }
        }
<<<<<<< HEAD
    }
}
=======
 }

>>>>>>> c81b4e6329e6483efa063ea1a35f4b70757d01e8
