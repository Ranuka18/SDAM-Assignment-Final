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
    public partial class AddReviewForm : Form
    {
        private int buyerId;
        private int productId;
        public int Rating { get; private set; }
        public string Comment { get; private set; }

        public AddReviewForm(int buyerId, int productId)
        {
            InitializeComponent();
            FormStyler.ApplyTheme(this);
            this.buyerId = buyerId;
            this.productId = productId;
        }

        private void AddReviewForm_Load(object sender, EventArgs e)
        {
            numRating.Minimum = 1;
            numRating.Maximum = 5;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // Get values from controls
            int rating = (int)numRating.Value;
            string comment = txtComment.Text.Trim();

            // Validate input
            if (string.IsNullOrWhiteSpace(comment))
            {
                MessageBox.Show("Please enter your review comment.");
                return;
            }

            // Store values in properties
            this.Rating = rating;
            this.Comment = comment;

            // Attempt to add review
            bool success = Review.AddReview(buyerId, productId, rating, comment);

            // Handle result
            if (success)
            {
                MessageBox.Show("Review submitted successfully!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to submit review. Please try again.");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
