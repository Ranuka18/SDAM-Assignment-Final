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
using SDAM_Assignment.Controllers;


namespace SDAM_Assignment
{
    public partial class EditProductForm : Form
    {
        private Product product;
        private byte[] newImageData = null;

        public EditProductForm(Product product)
        {
            InitializeComponent();
            FormStyler.ApplyTheme(this);
            this.product = product;

            txtName.Text = product.Name;
            txtPrice.Text = product.Price.ToString("F2");
            txtDescription.Text = product.Description;

            // Load existing image from byte array if available
            if (product.Image_data != null && product.Image_data.Length > 0)
            {
                using (var ms = new MemoryStream(product.Image_data))
                {
                    pictureBoxImage.Image = Image.FromStream(ms);
                }
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

            // Update image data if new image was selected
            if (newImageData != null)
                product.Image_data = newImageData;

            bool updated = ProductController.Update(product);

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
                    try
                    {
                        // Read the selected image into byte array
                        newImageData = File.ReadAllBytes(dialog.FileName);

                        // Display the image preview
                        using (var ms = new MemoryStream(newImageData))
                        {
                            pictureBoxImage.Image = Image.FromStream(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading image: " + ex.Message);
                    }
                }
            }
        }
    }
}
