using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.DB
{
    public abstract class DbConnection
    {
        private readonly string connectionstring;

        public DbConnection()
        {
            connectionstring = @"Data Source=MOHAMEDIJLAL\SQLEXPRESS;Initial Catalog=TechComPakistan;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(connectionstring); 
        }
    }
}
