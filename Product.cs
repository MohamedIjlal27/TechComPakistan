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
    public partial class Product : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string title = "TechComPakistan";
        public Product()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            LoadProduct();
        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            ProductModule module = new ProductModule(this);
            module.ShowDialog();
        }

        private void searchtxt_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dgvProduct.Columns[e.ColumnIndex].Name;
            if (ColName == "Edit")
            {
                ProductModule module = new ProductModule(this);
                DataGridViewRow selectedRow = dgvProduct.Rows[e.RowIndex];
                module.lblpcode.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.nametxt.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.cmbcategory.Text = dgvProduct.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.qtytext.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.pricetxt.Text = dgvProduct.Rows[e.RowIndex].Cells[5].Value.ToString();

                if (selectedRow.Cells[8].Value is DateTime dateValue)
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

                selectedRow.Cells[1].Value = module.lblpcode.Text;
                selectedRow.Cells[2].Value = module.nametxt.Text;
                selectedRow.Cells[3].Value = module.cmbcategory.Text;
                selectedRow.Cells[4].Value = module.qtytext.Text;
                selectedRow.Cells[5].Value = module.pricetxt.Text;
                selectedRow.Cells[6].Value = module.datetime.Value.ToString();
            }
            else if (ColName == "Delete")
            {
                if(MessageBox.Show("Are You sure you want to delete this record?","Delete Record",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    cn.Open();
                    cm = new SqlCommand("DELETE FROM ProductTable WHERE pcode LIKE '" + dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Item has been successfully deleted", title,MessageBoxButtons.OK, MessageBoxIcon.Question);

                }
            }

            LoadProduct();
        }

        #region Method
        public void LoadProduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM ProductTable WHERE CONCAT(PName, PCategory, PQty, PPrice, Date)Like'%" + searchtxt.Text + "%'AND PQty>0 ", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            cn.Close();

        }
        #endregion Method
    }
}
