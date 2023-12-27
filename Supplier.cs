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
    public partial class Supplier : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string title = "TechComPakistan";
        public Supplier()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            LoadSupplier();
        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            SupplierModel SM = new SupplierModel(this);
            SM.ShowDialog();
        }

        private void searchtxt_TextChanged(object sender, EventArgs e)
        {
            LoadSupplier();
        }

        private void dgvuser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dgvsupplier.Columns[e.ColumnIndex].Name;
            if (ColName == "Edit")
            {
                SupplierModel module = new SupplierModel(this);
                DataGridViewRow selectedRow = dgvsupplier.Rows[e.RowIndex];
                module.lblsid.Text = dgvsupplier.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.pcodecmbo.Text = dgvsupplier.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.producttxt.Text = dgvsupplier.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.suppliertxt.Text = dgvsupplier.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.qtytxt.Text = dgvsupplier.Rows[e.RowIndex].Cells[5].Value.ToString();                
                module.addresstxt.Text = dgvsupplier.Rows[e.RowIndex].Cells[6].Value.ToString();
                module.phonetxt.Text = dgvsupplier.Rows[e.RowIndex].Cells[7].Value.ToString();
                module.emailtxt.Text = dgvsupplier.Rows[e.RowIndex].Cells[8].Value.ToString();

                if (selectedRow.Cells[9].Value is DateTime dateValue)
                {
                    module.datetime.Value = dateValue;
                }
                else
                {
                    module.datetime.Value = DateTime.Now;
                }


                module.savebtn.Enabled = false;
                module.Updatebtn.Enabled = true;
                module.ShowDialog();

                selectedRow.Cells[1].Value = module.lblsid.Text;
                selectedRow.Cells[2].Value = module.pcodecmbo.Text;
                selectedRow.Cells[3].Value = module.producttxt.Text;
                selectedRow.Cells[4].Value = module.phonetxt.Text;
                selectedRow.Cells[5].Value = module.suppliertxt.Text;
                selectedRow.Cells[6].Value = module.qtytxt.Text;
                selectedRow.Cells[7].Value = module.phonetxt.Text;
                selectedRow.Cells[8].Value = module.emailtxt.Text;
                selectedRow.Cells[9].Value = module.datetime.Value.ToString();
            }
            else if (ColName == "Delete")
            {
                if (MessageBox.Show("Are You sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    cn.Open();
                    cm = new SqlCommand("DELETE FROM SupplierTable WHERE SID LIKE '" + dgvsupplier.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Item has been successfully deleted", title, MessageBoxButtons.OK, MessageBoxIcon.Question);

                }
            }
            LoadSupplier();
        }

        #region Method
        public void LoadSupplier()
        {
            int i = 0;
            dgvsupplier.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM SupplierTable WHERE CONCAT(SID, Pcode, ProductName, SupplierName, Quantity, Address, Phone, Email, Date)Like'%" + searchtxt.Text + "%'", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvsupplier.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString());
            }
            dr.Close();
            cn.Close();

        }
        #endregion Method
    }
}
