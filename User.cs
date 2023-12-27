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
    public partial class User : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string title = "TechComPakistan";
        public User()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            LoadUser();
        }

        private void searchtxt_TextChanged(object sender, EventArgs e)
        {
            LoadUser();
        }

        private void addbtn_Click(object sender, EventArgs e)  
        {
            UserModule module = new UserModule(this);
            module.ShowDialog();
        }

        private void dgvuser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dgvuser.Columns[e.ColumnIndex].Name;
            if( ColName == "Edit")
            {
                UserModule module = new UserModule(this);
                DataGridViewRow selectedRow = dgvuser.Rows[e.RowIndex];
                module.lbluid.Text = dgvuser.Rows[e.RowIndex].Cells[1].Value?.ToString();
                module.nametxt.Text = dgvuser.Rows[e.RowIndex].Cells[2].Value?.ToString();
                module.addresstxt.Text = dgvuser.Rows[e.RowIndex].Cells[3].Value?.ToString();
                module.phonetxt.Text = dgvuser.Rows[e.RowIndex].Cells[4].Value?.ToString();
                module.rolecmb.Text = dgvuser.Rows[e.RowIndex].Cells[5].Value?.ToString();
                module.dobdt.Text = dgvuser.Rows[e.RowIndex].Cells[6].Value?.ToString();
                module.txtpassword.Text = dgvuser.Rows[e.RowIndex].Cells[7].Value?.ToString();

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
                module.picbox.Visible = false;
                module.picture.Visible = false;

                module.ShowDialog();

                selectedRow.Cells[1].Value = module.lbluid.Text;
                selectedRow.Cells[2].Value = module.nametxt.Text;
                selectedRow.Cells[3].Value = module.addresstxt.Text;
                selectedRow.Cells[4].Value = module.phonetxt.Text;
                selectedRow.Cells[5].Value = module.rolecmb.Text;
                selectedRow.Cells[6].Value = module.dobdt.Text;
                selectedRow.Cells[7].Value = module.txtpassword.Text;
                selectedRow.Cells[8].Value = module.datetime.Value.ToString();



            }

            else if(ColName=="Delete")
            {
                if(MessageBox.Show("Are you sure you want to delete this record?","Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("DELETE FROM UserInfo WHERE id LIKE'" + dgvuser.Rows[e.RowIndex].Cells[1].Value.ToString() + "'",cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("User Data Deleted Successfully",title,MessageBoxButtons.OK,MessageBoxIcon.Question);
                }
            }
            LoadUser();
        }


        #region Method
        public void LoadUser()
        {
            int i = 0;
            dgvuser.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM UserInfo WHERE CONCAT(Name, Address, Phone, DOB, Role, Date)Like'%" + searchtxt.Text + "%'", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvuser.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), DateTime.Parse(dr[5].ToString()).ToShortDateString(), dr[6].ToString(), dr[7].ToString());
            }
            dr.Close();
            cn.Close();

        }

        #endregion Method

    }
}