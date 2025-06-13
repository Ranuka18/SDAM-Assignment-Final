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
    public partial class AdminViewReviewsForm : Form
    {
        private int productId;

        public AdminViewReviewsForm(int productId)
        {
            InitializeComponent();
            FormStyler.ApplyTheme(this);

            this.productId = productId;
            LoadReviews();
        }

        private void LoadReviews()
        {
            flowLayoutPanelReviews.Controls.Clear();
            var reviews = Review.GetReviews(productId);

            foreach (var review in reviews)
            {
                Panel panel = new Panel { Width = 350, Height = 100, Tag = "NoTheme", BackColor = Color.WhiteSmoke, BorderStyle = BorderStyle.FixedSingle, Margin = new Padding(5) };

                Label lbl = new Label
                {
                    Text = $"Buyer ID: {review.BuyerId}\nRating: {review.Rating}/5\nComment: {review.Comment}",
                    AutoSize = true,
                    Left = 10,
                    Top = 10
                };

                Button btnDelete = new Button
                {
                    Text = "Delete",
                    Left = 250,
                    Top = 10,
                    BackColor = Color.Red
                };

                btnDelete.Click += (s, e) =>
                {
                    Review.DeleteReview(review.ReviewId);
                    LoadReviews();
                };

                panel.Controls.Add(lbl);
                panel.Controls.Add(btnDelete);
                flowLayoutPanelReviews.Controls.Add(panel);
            }
        }
    }
}