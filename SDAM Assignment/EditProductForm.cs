using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SDAM_Assignment.Helpers;


namespace SDAM_Assignment
{
    public partial class EditProductForm : Form
    {
        private Product product;
        private string newImagePath = null;


        public EditProductForm(Product product)
        {
            InitializeComponent();
            FormStyler.ApplyTheme(this);
            this.product = product;

            txtName.Text = product.Name;
            txtPrice.Text = product.Price.ToString("F2");
            txtDescription.Text = product.Description;

            if (!string.IsNullOrEmpty(product.ImagePath) && File.Exists(product.ImagePath))
            {
                pictureBoxImage.Image = Image.FromFile(product.ImagePath);
            }
            else
            {
                pictureBoxImage.Image = null;
            }
        }

      

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtName.Text))
                product.Name = txtName.Text.Trim();

            if (!string.IsNullOrWhiteSpace(txtDescription.Text))
                product.Description = txtDescription.Text.Trim();

            if (decimal.TryParse(txtPrice.Text.Trim(), out decimal price))
                product.Price = price;

            if (!string.IsNullOrEmpty(newImagePath))
                product.ImagePath = newImagePath;

            bool updated = Product.Update(product);

            if (updated)
            {
                MessageBox.Show("Product updated successfully!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to update product.");
            }
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Select Product Image";
                dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    newImagePath = dialog.FileName;
                    pictureBoxImage.Image = Image.FromFile(newImagePath);
                }
            }
        }
    }
}
