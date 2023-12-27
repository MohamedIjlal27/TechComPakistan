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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace WindowsFormsApp1
{
    public partial class SalesCustomerAdd : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string title = "TechComPakistan";
        bool check = false;
        SalesCustomer sacustomer;
        public SalesCustomerAdd(SalesCustomer form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            sacustomer = form;
        }

        private void savebtn_Click(object sender, EventArgs e)
        {
            try
            {
                checkfield();
                if (check)
                {
                    if (MessageBox.Show("Are you sure want to register this Customer?", "Customer Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("INSERT INTO CustomerTable(Name, Address, Phone, Date)VALUES(@Name, @Address, @Phone, @Date)", cn);
                        cm.Parameters.AddWithValue("@Name", nametxt.Text);
                        cm.Parameters.AddWithValue("@Address", addresstxt.Text);
                        cm.Parameters.AddWithValue("@Phone", phonetxt.Text);
                        cm.Parameters.AddWithValue("@Date", datebox.Value);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Customer has been registered Successfully", title);
                        clear();
                        sacustomer.LoadCustomer();
                        this.Dispose();

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }
        
        private void cancelbtn_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }



        #region Method

        public void clear()
        {
            nametxt.Clear();
            addresstxt.Clear();
            phonetxt.Clear();

            savebtn.Enabled = true;
        }

        public void checkfield()
        {
            if (nametxt.Text == "" | addresstxt.Text == "" | phonetxt.Text == "")
            {
                MessageBox.Show("Fill the required Data Field!, WARNING");
                return;
            }

            check = true;
        }
        #endregion Method
    }
}
