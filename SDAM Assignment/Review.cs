using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SDAM_Assignment
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int BuyerId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }

        private static string connectionString = "server=localhost;user=root;password=;database=marketplace;";


        public static bool AddReview(int buyerId, int productId, int rating, string comment)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Check if review exists
                string checkQuery = "SELECT COUNT(*) FROM reviews WHERE buyer_id = @buyerId AND product_id = @productId";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@buyerId", buyerId);
                checkCmd.Parameters.AddWithValue("@productId", productId);
                bool alreadyReviewed = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;

                if (alreadyReviewed)
                {
                    return false;
                }

                // Insert new review
                string insertQuery = "INSERT INTO reviews (buyer_id, product_id, rating, comment) VALUES (@buyerId, @productId, @rating, @comment)";
                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@buyerId", buyerId);
                cmd.Parameters.AddWithValue("@productId", productId);
                cmd.Parameters.AddWithValue("@rating", rating);
                cmd.Parameters.AddWithValue("@comment", comment);
                return cmd.ExecuteNonQuery() > 0;
            }
        }



        public static List<Review> GetReviews(int productId)
        {
            List<Review> list = new List<Review>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM reviews WHERE product_id = @productId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@productId", productId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Review
                        {
                            ReviewId = Convert.ToInt32(reader["review_id"]),
                            BuyerId = Convert.ToInt32(reader["buyer_id"]),
                            ProductId = Convert.ToInt32(reader["product_id"]),
                            Rating = Convert.ToInt32(reader["rating"]),
                            Comment = reader["comment"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public static bool DeleteReview(int reviewId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM reviews WHERE review_id = @reviewId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@reviewId", reviewId);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

    }
}
