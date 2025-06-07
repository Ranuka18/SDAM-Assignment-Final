using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SDAM_Assignment
{
    public class Database
    {
        public static MySqlConnection GetConnection()
        {
            string connStr = "server=localhost;user=root;database=marketplace;password=;";
            return new MySqlConnection(connStr);
        }
    }
}
