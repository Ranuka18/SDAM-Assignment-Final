using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace SDAM_Assignment
{
    class Order
    {
        public int OrderId { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string ShippingAddress { get; set; }
        public string CancelReason { get; set; }

        private static string connectionString = "server=localhost;user=root;password=;database=marketplace;";

        public static bool PlaceOrder(int buyerId, int productId, string shippingAddress, string status)
        {
            try
            {
                List<CartItem> cartItems = CartItem.GetCartItems(buyerId);

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (var item in cartItems)
                    {
                        Product product = Product.GetById(item.ProductId);
                        if (product == null) continue;

                        string insertQuery = "INSERT INTO orders (buyer_id, seller_id, product_id, quantity, status, shipping_address) " +
                                             "VALUES (@buyerId, @sellerId, @productId, @quantity, @status, @shipping)";

                        using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@buyerId", buyerId);
                            cmd.Parameters.AddWithValue("@sellerId", product.SellerId);
                            cmd.Parameters.AddWithValue("@productId", item.ProductId);
                            cmd.Parameters.AddWithValue("@quantity", item.Quantity);
                            cmd.Parameters.AddWithValue("@status", status);
                            cmd.Parameters.AddWithValue("@shipping", shippingAddress);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    CartItem.ClearCart(buyerId);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Order Error: " + ex.Message);
                return false;
            }
        }

        public static List<Order> GetOrdersForSeller(int sellerId)
        {
            List<Order> orders = new List<Order>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = @"SELECT o.order_id, o.buyer_id, o.product_id, o.status, o.shipping_address, o.cancel_reason
                                 FROM orders o
                                 JOIN products p ON o.product_id = p.product_id
                                 WHERE p.seller_id = @sellerId";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@sellerId", sellerId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                OrderId = Convert.ToInt32(reader["order_id"]),
                                BuyerId = Convert.ToInt32(reader["buyer_id"]),
                                ProductId = Convert.ToInt32(reader["product_id"]),
                                Status = reader["status"].ToString(),
                                ShippingAddress = reader["shipping_address"].ToString(),
                                CancelReason = reader["cancel_reason"] == DBNull.Value ? "" : reader["cancel_reason"].ToString()
                            });
                        }
                    }
                }
            }

            return orders;
        }

        public static List<Order> GetOrdersForBuyer(int buyerId)
        {
            List<Order> orders = new List<Order>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM orders WHERE buyer_id = @buyerId";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@buyerId", buyerId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                OrderId = Convert.ToInt32(reader["order_id"]),
                                BuyerId = Convert.ToInt32(reader["buyer_id"]),
                                ProductId = Convert.ToInt32(reader["product_id"]),
                                Status = reader["status"].ToString(),
                                ShippingAddress = reader["shipping_address"].ToString(),
                                CancelReason = reader["cancel_reason"] == DBNull.Value ? "" : reader["cancel_reason"].ToString()
                            });
                        }
                    }
                }
            }

            return orders;
        }

        public static void UpdateStatus(int orderId, string status, string reason = null)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE orders SET status = @status, cancel_reason = @reason WHERE order_id = @id";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@reason", reason ?? "");
                    cmd.Parameters.AddWithValue("@id", orderId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}