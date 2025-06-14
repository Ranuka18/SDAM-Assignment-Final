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
using SDAM_Assignment.Controllers;
using SDAM_Assignment.Helpers;

namespace SDAM_Assignment
{
    public partial class AddProductForm : Form
    {
        private string imagePath = "";
        private int sellerId;
        private Seller seller;

        public AddProductForm(int sellerId)
        {
            InitializeComponent();
            FormStyler.ApplyTheme(this);
            this.sellerId = sellerId;
            this.seller = SellerController.GetSellerById(sellerId);

            // Remove SellerController instance since we're using static methods
            btnUploadImage.Click += btnUploadImage_Click;
            btnSave.Click += btnSave_Click;
        }

        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    imagePath = ofd.FileName;
                    try
                    {
                        pictureBox1.Image = Image.FromFile(imagePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}");
                        imagePath = "";
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Input validation
            if (!ValidateInputs(out string name, out string desc, out decimal price))
            {
                return;
            }

            // Use static SellerController
            bool result = SellerController.AddProduct(sellerId, name, desc, price, imagePath);

            if (result)
            {
                MessageBox.Show("Product added successfully.");
                ResetForm();
            }
            else
            {
                MessageBox.Show("Failed to add product.");
            }
        }

        private bool ValidateInputs(out string name, out string desc, out decimal price)
        {
            name = txtName.Text.Trim();
            desc = txtDescription.Text.Trim();
            string priceStr = txtPrice.Text.Trim();
            price = 0;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter product name.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(desc))
            {
                MessageBox.Show("Please enter product description.");
                return false;
            }

            if (!decimal.TryParse(priceStr, out price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price (must be positive number).");
                return false;
            }

            return true;
        }

        private void ResetForm()
        {
            txtName.Clear();
            txtDescription.Clear();
            txtPrice.Clear();
            pictureBox1.Image = null;
            imagePath = "";
        }
    }
}