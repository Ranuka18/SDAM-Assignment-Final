using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;

namespace SDAM_Assignment.Controllers
{
    public static class OrderController
    {
        private static readonly string connectionString = "server=localhost;user=root;password=;database=marketplace;";

        public static bool PlaceOrder(int buyerId, int productId, string shippingAddress, string status)
        {
            try
            {
                List<CartItem> cartItems = CartItemsController.GetCartItems(buyerId);

                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (var item in cartItems)
                    {
                        Product product = ProductController.GetById(item.ProductId);
                        if (product == null) continue;

                        string insertQuery = @"INSERT INTO orders 
                            (buyer_id, seller_id, product_id, quantity, status, shipping_address) 
                            VALUES (@buyerId, @sellerId, @productId, @quantity, @status, @shipping)";

                        using (var cmd = new MySqlCommand(insertQuery, conn))
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

                    CartItemsController.ClearCart(buyerId);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Order Error: " + ex.Message);
                return false;
            }
        }

        public static int GetOrderCountForProduct(int productId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM orders WHERE product_id = @productId";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@productId", productId);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public static DataTable GetOrdersByProduct(int productId)
        {
            var dt = new DataTable();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT o.order_id, o.buyer_id, p.name AS product_name, 
                          o.quantity, o.status, o.shipping_address
                          FROM orders o
                          JOIN products p ON o.product_id = p.product_id
                          WHERE o.product_id = @productId";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@productId", productId);
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public static bool ResolveAllOrdersForProduct(int productId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"UPDATE orders SET status = 'Completed' 
                          WHERE product_id = @productId AND status != 'Cancelled'";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@productId", productId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static List<Order> GetOrdersForProduct(int productId)
        {
            var orders = new List<Order>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT * FROM orders WHERE product_id = @productId";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@productId", productId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                OrderId = reader.GetInt32("order_id"),
                                BuyerId = reader.GetInt32("buyer_id"),
                                ProductId = productId,
                                Status = reader.GetString("status"),
                                ShippingAddress = reader.GetString("shipping_address"),
                                CancelReason = reader["cancel_reason"] == DBNull.Value ? null : reader.GetString("cancel_reason")
                            });
                        }
                    }
                }
            }
            return orders;
        }



        public static List<Order> GetOrdersForSeller(int sellerId)
        {
            var orders = new List<Order>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT o.order_id, o.buyer_id, o.product_id, o.status, 
                              o.shipping_address, o.cancel_reason
                              FROM orders o
                              JOIN products p ON o.product_id = p.product_id
                              WHERE p.seller_id = @sellerId";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@sellerId", sellerId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                OrderId = reader.GetInt32("order_id"),
                                BuyerId = reader.GetInt32("buyer_id"),
                                ProductId = reader.GetInt32("product_id"),
                                Status = reader.GetString("status"),
                                ShippingAddress = reader.GetString("shipping_address"),
                                CancelReason = reader["cancel_reason"] == DBNull.Value ? "" : reader.GetString("cancel_reason")
                            });
                        }
                    }
                }
            }
            return orders;
        }

        public static List<Order> GetOrdersForBuyer(int buyerId)
        {
            var orders = new List<Order>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM orders WHERE buyer_id = @buyerId";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@buyerId", buyerId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                OrderId = reader.GetInt32("order_id"),
                                BuyerId = buyerId,
                                ProductId = reader.GetInt32("product_id"),
                                Status = reader.GetString("status"),
                                CancelReason = reader["cancel_reason"] == DBNull.Value ? null : reader.GetString("cancel_reason")
                            });
                        }
                    }
                }
            }
            return orders;
        }

        public static bool UpdateStatus(int orderId, string status, string reason = null)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"UPDATE orders SET status = @status, 
                              cancel_reason = @reason 
                              WHERE order_id = @orderId";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@reason", reason ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@orderId", orderId);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}