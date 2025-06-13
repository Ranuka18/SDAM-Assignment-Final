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
    public partial class AddProductForm : Form
    {
        private string imagePath = "";
        private Seller seller;

        public AddProductForm(int sellerId)
        {
            InitializeComponent();
            FormStyler.ApplyTheme(this);
            this.seller = Seller.GetSellerById(sellerId);

            btnUploadImage.Click += btnUploadImage_Click;
            btnSave.Click += btnSave_Click;
        }

        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                imagePath = ofd.FileName;
                pictureBox1.Image = Image.FromFile(imagePath);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string desc = txtDescription.Text.Trim();
            string priceStr = txtPrice.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(desc) || string.IsNullOrWhiteSpace(priceStr))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            if (!decimal.TryParse(priceStr, out decimal price))
            {
                MessageBox.Show("Price must be a number.");
                return;
            }

            bool result = seller.AddProduct(name, desc, price, imagePath);

            if (result)
            {
                MessageBox.Show("Product added successfully.");
                txtName.Clear();
                txtDescription.Clear();
                txtPrice.Clear();
                pictureBox1.Image = null;
                imagePath = "";
            }
            else
            {
                MessageBox.Show("Failed to add product.");
            }
        }
    }
}