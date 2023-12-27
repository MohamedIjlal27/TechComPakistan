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
    public partial class ProductModule : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        string title = "TechComPakistan";
        bool check = false;
        Product product;
        public ProductModule(Product form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            product = form;
            cmbcategory.SelectedIndex = 0;
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void savebtn_Click(object sender, EventArgs e)
        {
            try
            {
               checkfield();
                if (check)
                {
                    if (MessageBox.Show("Are you sure want to Add this Product?", "Product Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("INSERT INTO ProductTable(PName, PCategory, PQty, PPrice, Date)VALUES(@PName, @PCategory, @PQty, @PPrice, @Date)", cn);
                        cm.Parameters.AddWithValue("@PName", nametxt.Text);
                        cm.Parameters.AddWithValue("@PCategory", cmbcategory.Text);
                        cm.Parameters.AddWithValue("@PQty", qtytext.Text);
                        cm.Parameters.AddWithValue("@PPrice",double.Parse(pricetxt.Text));
                        cm.Parameters.AddWithValue("@Date", datetime.Value);


                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Product has been registered Successfully", title);
                        Clear();
                        product.LoadProduct();
                        this.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                cn.Close();
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
                    if (MessageBox.Show("Are you sure want to Update this Data?", "Product Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("UPDATE ProductTable SET PName=@PName, PCategory=@PCategory, PQty=@PQty, PPrice=@PPrice, Date=@Date WHERE Pcode=@Pcode", cn);
                        cm.Parameters.AddWithValue("@Pcode", lblpcode.Text);
                        cm.Parameters.AddWithValue("@PName", nametxt.Text);
                        cm.Parameters.AddWithValue("@PCategory", cmbcategory.Text);
                        cm.Parameters.AddWithValue("@PQty", int.Parse(qtytext.Text));
                        cm.Parameters.AddWithValue("@PPrice", double.Parse(pricetxt.Text));
                        cm.Parameters.AddWithValue("@Date", datetime.Value);


                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Product has been Updated Successfully", title);
                        product.LoadProduct();
                        this.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
        }

        private void cancelbtn_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void qtytext_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void pricetxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if((e.KeyChar=='.')&&((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        #region Method
        public void Clear()
        {
            nametxt.Clear();
            pricetxt.Clear();
            cmbcategory.SelectedIndex = 0;
            qtytext.Clear();


            Updatebtn.Enabled = false;
        }

        public void checkfield()
        {
            if (nametxt.Text == "" | pricetxt.Text == "" | qtytext.Text == "")
            {

                MessageBox.Show("Fill the required Data Field!, WARNING");
                return;
            }
            check = true;
        }
        #endregion Method
    }
}
