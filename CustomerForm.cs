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
    public partial class CustomerForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string title = "TechComPakistan";
        public CustomerForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            LoadCustomer();
        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            CustomerModule module = new CustomerModule(this);
            module.ShowDialog();
        }

        private void searchtxt_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer();
        }

        private void dgvcustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dgvcustomer.Columns[e.ColumnIndex].Name;
            if (ColName == "Edit")
            {
                CustomerModule module = new CustomerModule(this);
                DataGridViewRow selectedRow = dgvcustomer.Rows[e.RowIndex];
                module.lblcid.Text = selectedRow.Cells[1].Value?.ToString();
                module.nametxt.Text = selectedRow.Cells[2].Value?.ToString();
                module.addresstxt.Text = selectedRow.Cells[3].Value?.ToString();
                module.phonetxt.Text = selectedRow.Cells[4].Value?.ToString();

                if (selectedRow.Cells[5].Value is DateTime dateValue)
                {
                    module.datebox.Value = dateValue;
                }
                else
                {
                    module.datebox.Value = DateTime.Now; 
                }

                module.savebtn.Enabled = false;
                module.Updatebtn.Enabled = true;
                module.ShowDialog();

               
                selectedRow.Cells[1].Value = module.lblcid.Text;
                selectedRow.Cells[2].Value = module.nametxt.Text;
                selectedRow.Cells[3].Value = module.addresstxt.Text;
                selectedRow.Cells[4].Value = module.phonetxt.Text;
                selectedRow.Cells[5].Value = module.datebox.Value.ToString();
            }

            else if (ColName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this Customer record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM CustomerTable WHERE id LIKE'" + dgvcustomer.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Customer Data Deleted Successfully", title, MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
            }
            LoadCustomer();
        }

        #region Method
        public void LoadCustomer()
        {
            int i = 0;
            dgvcustomer.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM CustomerTable WHERE CONCAT(Name, Address, Phone, Date)Like'%" + searchtxt.Text + "%'", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvcustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString());
            }
            dr.Close();
            cn.Close();

        }


        #endregion Method
    }
}
