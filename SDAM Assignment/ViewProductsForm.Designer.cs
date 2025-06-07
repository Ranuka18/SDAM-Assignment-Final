using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace SDAM_Assignment
{
    partial class ViewProductsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanelProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // flowLayoutPanelProducts
            // 
            this.flowLayoutPanelProducts.Location = new System.Drawing.Point(39, 91);
            this.flowLayoutPanelProducts.Name = "flowLayoutPanelProducts";
            this.flowLayoutPanelProducts.Size = new System.Drawing.Size(1724, 718);
            this.flowLayoutPanelProducts.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(34, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "My Products";
            // 
            // ViewProductsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1788, 821);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flowLayoutPanelProducts);
            this.Name = "ViewProductsForm";
            this.Text = "ViewProductsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Panel CreateProductCard(string name, string description, decimal price, string imagePath)
        {
            Panel card = new Panel
            {
                Width = 200,
                Height = 300,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10)
            };

            PictureBox picture = new PictureBox
            {
                Width = 180,
                Height = 150,
                Top = 10,
                Left = 10,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = File.Exists(imagePath) ? Image.FromFile(imagePath) : null
            };

            Label lblName = new Label
            {
                Text = name,
                Top = picture.Bottom + 5,
                Left = 10,
                Width = 180,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Label lblDesc = new Label
            {
                Text = description,
                Top = lblName.Bottom + 5,
                Left = 10,
                Width = 180,
                Height = 40,
                AutoEllipsis = true
            };

            Label lblPrice = new Label
            {
                Text = $"${price:F2}",
                Top = lblDesc.Bottom + 5,
                Left = 10,
                Width = 180,
                ForeColor = Color.Green,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            card.Controls.Add(picture);
            card.Controls.Add(lblName);
            card.Controls.Add(lblDesc);
            card.Controls.Add(lblPrice);

            return card;
        }



        #endregion

        private FlowLayoutPanel flowLayoutPanelProducts;
        private Label label1;
    }
}