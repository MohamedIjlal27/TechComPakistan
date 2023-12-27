using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal class DBConnect
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        private string con;

        public string connection()
        {
            con = @"Data Source=DESKTOP-SD2A8O9;Initial Catalog=TechComPakistan;Integrated Security=True";
            return con;
        }

        public void executeQuery(string sql)
        {
            try
            {
                cn.ConnectionString = connection();
                cn.Open();
                cmd = new SqlCommand(sql, cn);
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
