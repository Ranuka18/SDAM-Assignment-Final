using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace SDAM_Assignment
{
    public class Order
    {
        public int OrderId { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string ShippingAddress { get; set; }
        public string CancelReason { get; set; }
    }
}