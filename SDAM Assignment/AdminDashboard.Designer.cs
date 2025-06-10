namespace SDAM_Assignment
{
    partial class AdminDashboard
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnViewSellers = new System.Windows.Forms.Button();
            this.btnViewBuyers = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Welcome, Admin";
            // 
            // btnViewSellers
            // 
            this.btnViewSellers.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewSellers.Location = new System.Drawing.Point(78, 119);
            this.btnViewSellers.Name = "btnViewSellers";
            this.btnViewSellers.Size = new System.Drawing.Size(141, 30);
            this.btnViewSellers.TabIndex = 1;
            this.btnViewSellers.Text = "View Sellers";
            this.btnViewSellers.UseVisualStyleBackColor = true;
            this.btnViewSellers.Click += new System.EventHandler(this.btnViewSellers_Click);
            // 
            // btnViewBuyers
            // 
            this.btnViewBuyers.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewBuyers.Location = new System.Drawing.Point(282, 119);
            this.btnViewBuyers.Name = "btnViewBuyers";
            this.btnViewBuyers.Size = new System.Drawing.Size(141, 30);
            this.btnViewBuyers.TabIndex = 2;
            this.btnViewBuyers.Text = "View Buyers";
            this.btnViewBuyers.UseVisualStyleBackColor = true;
            this.btnViewBuyers.Click += new System.EventHandler(this.btnViewBuyers_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.Location = new System.Drawing.Point(421, 31);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(107, 37);
            this.btnLogout.TabIndex = 3;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // AdminDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 229);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnViewBuyers);
            this.Controls.Add(this.btnViewSellers);
            this.Controls.Add(this.label1);
            this.Name = "AdminDashboard";
            this.Text = "AdminDashboard";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnViewSellers;
        private System.Windows.Forms.Button btnViewBuyers;
        private System.Windows.Forms.Button btnLogout;
    }
}