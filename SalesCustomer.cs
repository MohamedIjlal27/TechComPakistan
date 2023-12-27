using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class SalesCustomer : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string title = "TechComPakistan";
        SalesForm cash;
        public SalesCustomer(SalesForm form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            cash = form;
            LoadCustomer();
        }

        private void searchtxt_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer();
        }       

        private void dgvcustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dgvcustomer.Columns[e.ColumnIndex].Name;
            if (ColName == "Choice")
            {
                dbcon.executeQuery("UPDATE SalesTable SET CID=" + dgvcustomer.Rows[e.RowIndex].Cells[1].Value.ToString()+ "WHERE TransNo = "+cash.lblTransaction.Text+"");
                cash.LoadCash();
                this.Dispose();
            }


        }
        private void addbtn_Click(object sender, EventArgs e)
        {
            
        }

        

        private void exit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void addbtn_Click_1(object sender, EventArgs e)
        {
            SalesCustomerAdd SCA = new SalesCustomerAdd(this);
            SCA.ShowDialog();
        }

       


        #region Method
        public void LoadCustomer()
        {


            try
            {
                int i = 0;
                dgvcustomer.Rows.Clear();
                cm = new SqlCommand("SELECT ID, Name, Phone FROM CustomerTable WHERE Name Like'%" + searchtxt.Text + "%'", cn);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvcustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        #endregion Method

        private void btnsubmit_Click(object sender, EventArgs e)
        {

        }
    }
}
