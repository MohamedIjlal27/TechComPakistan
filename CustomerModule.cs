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
    public partial class CustomerModule : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        string title = "TechComPakistan";

        bool check = false;
        CustomerForm customer;
        public CustomerModule(CustomerForm form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            customer = form;
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
                        customer.LoadCustomer();
                        this.Dispose();

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        private void Updatebtn_Click(object sender, EventArgs e)
        {
            try
            {
                checkfield();
                if (check)
                {
                    if (MessageBox.Show("Are you sure want to Update this Customer Record?", "Customer Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("UPDATE CustomerTable SET Name=@Name, Address=@Address, Phone=@Phone, Date=@Date WHERE ID=@ID", cn);
                        cm.Parameters.AddWithValue("@ID", lblcid.Text);
                        cm.Parameters.AddWithValue("@Name", nametxt.Text);
                        cm.Parameters.AddWithValue("@Address", addresstxt.Text);
                        cm.Parameters.AddWithValue("@Phone", phonetxt.Text);
                        cm.Parameters.AddWithValue("@Date", datebox.Value);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Customer data has been Successfully Updated ", title);
                        clear();
                        customer.LoadCustomer();
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
            Updatebtn.Enabled = false;
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
