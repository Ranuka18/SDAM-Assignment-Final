using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Seller
{
    public partial class ViewOrdersForm : Form
    {
        int sellerId;
        public ViewOrdersForm(int id)
        {
            InitializeComponent();
            sellerId = id;
        }

        private void ViewOrdersForm_Load(object sender, EventArgs e)
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            dgvOrders.Columns.Clear();

            var conn = Database.GetConnection();
            string query = @"
            SELECT orders.order_id, users.Name AS buyer_name, users.Phone_no,
                   orders.shipping_address, orders.status,
                   payments.method, payments.card_last4
            FROM orders
            JOIN products ON orders.product_id = products.product_id
            JOIN users ON orders.buyer_id = users.ID
            JOIN payments ON orders.order_id = payments.order_id
            WHERE products.seller_id = @sid";

            var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@sid", sellerId);
            var da = new MySqlDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);

            dgvOrders.DataSource = dt;

            // Add the "Ship" button column
            DataGridViewButtonColumn shipButton = new DataGridViewButtonColumn();
            shipButton.Name = "Ship";
            shipButton.HeaderText = "Action";
            shipButton.Text = "Mark as Shipped";
            shipButton.UseColumnTextForButtonValue = true;
            dgvOrders.Columns.Add(shipButton);

            // Add click handler
            dgvOrders.CellClick += dgvOrders_CellClick;
        }

        private void dgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if "Ship" button clicked
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvOrders.Columns["Ship"].Index)
            {
                int orderId = Convert.ToInt32(dgvOrders.Rows[e.RowIndex].Cells["order_id"].Value);

                var conn = Database.GetConnection();
                string update = "UPDATE orders SET status = 'Shipped' WHERE order_id = @oid";
                var cmd = new MySqlCommand(update, conn);
                cmd.Parameters.AddWithValue("@oid", orderId);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Order marked as Shipped!");

                LoadOrders(); // Refresh table
            }
        }
    }
}

