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
    public partial class ViewReviewsForm : Form
    {
        private int productId;

        public ViewReviewsForm(int productId)
        {
            InitializeComponent();
            FormStyler.ApplyTheme(this);
            this.productId = productId;
            LoadReviews();
        }

        private void ViewReviewsForm_Load(object sender, EventArgs e)
        {
            flowLayoutPanelReviews.Controls.Clear();

            List<Review> reviews = Review.GetReviews(productId);
            if (reviews.Count == 0)
            {
                Label lbl = new Label
                {
                    Text = "No reviews for this product yet.",
                    AutoSize = true
                };
                flowLayoutPanelReviews.Controls.Add(lbl);
                return;
            }

            foreach (var r in reviews)
            {
                Panel card = new Panel
                {
                    Width = 300,
                    Height = 100,
                    Tag = "NoTheme",
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(5),
                    BackColor = Color.White
                };

                Label lblRating = new Label
                {
                    Text = $"⭐ Rating: {r.Rating}/5",
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    Top = 10,
                    Left = 10,
                    AutoSize = true
                };

                Label lblComment = new Label
                {
                    Text = r.Comment,
                    Top = lblRating.Bottom + 5,
                    Left = 10,
                    Width = 270,
                    Height = 50,
                    AutoEllipsis = true
                };

                card.Controls.Add(lblRating);
                card.Controls.Add(lblComment);
                flowLayoutPanelReviews.Controls.Add(card);
            }
        }

        private void LoadReviews()
        {
            flowLayoutPanelReviews.Controls.Clear();
            var reviews = Review.GetReviews(productId);

            if (reviews.Count == 0)
            {
                flowLayoutPanelReviews.Controls.Add(new Label
                {
                    Text = "No reviews yet.",
                    AutoSize = true,
                    ForeColor = Color.Gray
                });
                return;
            }

            foreach (var review in reviews)
            {
                Label lbl = new Label
                {
                    Text = $"Rating: {review.Rating}/5\n{review.Comment}",
                    AutoSize = true,
                    BorderStyle = BorderStyle.FixedSingle,
                    Padding = new Padding(5),
                    Margin = new Padding(5)
                };

                flowLayoutPanelReviews.Controls.Add(lbl);
            }
        }

    }
}
