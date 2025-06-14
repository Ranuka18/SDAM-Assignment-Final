using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SDAM_Assignment
{
    public class CartItem
    {
        public int CartId { get; set; }
        public int BuyerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

       
    }
}
