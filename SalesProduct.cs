using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class SalesProduct : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string title = "TechComPakistan";
        public string uname;
        SalesForm cash;
        public SalesProduct(SalesForm form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            cash = form;
            LoadProduct();
        }

        private void btnsubmit_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow dr in dgvProduct.Rows)
            {
                bool chckbox = Convert.ToBoolean(dr.Cells["Select"].Value);
                if (chckbox)
                {
                    try
                    {
                        cm = new SqlCommand("INSERT INTO SalesTable(TransNo, PCode, PName, Qty, Price, Cashier) VALUES(@TransNo, @PCode, @PName, @Qty, @Price, @Cashier)", cn);
                        cm.Parameters.AddWithValue("@TransNo",cash.lblTransaction.Text);
                        cm.Parameters.AddWithValue("@PCode", dr.Cells[1].Value.ToString());
                        cm.Parameters.AddWithValue("@PName", dr.Cells[2].Value.ToString());
                        cm.Parameters.AddWithValue("@Qty", 1);
                        cm.Parameters.AddWithValue("@Price",Convert.ToDouble(dr.Cells[4].Value.ToString()));
                        cm.Parameters.AddWithValue("@Cashier", uname);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();
                    }
                    catch(Exception ex)
                    {
                        cn.Close();
                        MessageBox.Show(ex.Message,title);
                    }
                }
            }
            cash.LoadCash();
            this.Dispose();
        }

        private void searchtxt_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close(); ;
        }

        private void CashProduct_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=DESKTOP-SD2A8O9;Initial Catalog=TechComPakistan;Integrated Security=True
";
            string query = "SELECT Pcode FROM ProductTable WHERE PQty > 0";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string productCode = reader["Pcode"].ToString();
                        pcodecombo.Items.Add(productCode);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }


        }

        private void Pcodecombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=DESKTOP-SD2A8O9;Initial Catalog=TechComPakistan;Integrated Security=True";
            string selectedProductCode = pcodecombo.SelectedItem.ToString();
            string query = "SELECT PName, PCategory, PPrice FROM ProductTable WHERE Pcode = @Pcode";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Pcode", selectedProductCode);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string name = reader["PName"].ToString();
                        string category = reader["PCategory"].ToString();
                        decimal price = Convert.ToDecimal(reader["PPrice"]);

                        pnamtxt.Text = name;
                        categorytxt.Text = category;
                        pricetxt.Text = price.ToString();
                        sellingtxt.Text = pricetxt.Text;
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }        

        private void disc2_Click(object sender, EventArgs e)
        {
            double total;
            total = (2.0 / 100.0) * Convert.ToDouble(pricetxt.Text);
            discounttxt.Text = total.ToString();

            double price = Convert.ToDouble(pricetxt.Text);
            double discount = Convert.ToDouble(discounttxt.Text);
            double sellingPrice = price - discount;

            sellingtxt.Text = sellingPrice.ToString();

        }

        private void disc5_Click(object sender, EventArgs e)
        {
            double total;
            total = (5.0 / 100.0) * Convert.ToDouble(pricetxt.Text);
            discounttxt.Text = total.ToString();

            double price = Convert.ToDouble(pricetxt.Text);
            double discount = Convert.ToDouble(discounttxt.Text);
            double sellingPrice = price - discount;

            sellingtxt.Text = sellingPrice.ToString();
        }

        private void disc7_Click(object sender, EventArgs e)
        {
            double total;
            total = (7.0 / 100.0) * Convert.ToDouble(pricetxt.Text);
            discounttxt.Text = total.ToString();

            double price = Convert.ToDouble(pricetxt.Text);
            double discount = Convert.ToDouble(discounttxt.Text);
            double sellingPrice = price - discount;

            sellingtxt.Text = sellingPrice.ToString();
        }

        private void sellingtxt_TextChanged(object sender, EventArgs e)
        {


        }

        private void cancelbtn_Click(object sender, EventArgs e)
        {
            pcodecombo.Items.Clear();
            pnamtxt.Clear();
            categorytxt.Clear();
            pricetxt.Clear();
            discounttxt.Clear();
            sellingtxt.Clear();
        }

        #region Method
        public void LoadProduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            cm = new SqlCommand("SELECT Pcode, PName, PCategory, PPrice FROM ProductTable WHERE CONCAT(PName, PCategory, PPrice)Like'%" + searchtxt.Text + "%' AND PQty > " + 0 + "", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString());
            }
            dr.Close();
            cn.Close();

        }

        #endregion Method

        private void Addbtn_Click(object sender, EventArgs e)
        {
            try
            {
                cm = new SqlCommand("INSERT INTO SalesTable(TransNo, PCode, PName, Qty, OriginalPrice, DiscountPrice, SellingPrice, Cashier) VALUES(@TransNo, @PCode, @PName, @Qty, @OriginalPrice, @DiscountPrice, @SellingPrice, @Cashier)", cn);
                cm.Parameters.AddWithValue("@TransNo", cash.lblTransaction.Text);
                cm.Parameters.AddWithValue("@PCode", pcodecombo.Text);
                cm.Parameters.AddWithValue("@PName", pnamtxt.Text);
                cm.Parameters.AddWithValue("@Qty", 1);
                cm.Parameters.AddWithValue("@OriginalPrice", Convert.ToDouble(pricetxt.Text));
                cm.Parameters.AddWithValue("@DiscountPrice", Convert.ToDouble(discounttxt.Text));
                cm.Parameters.AddWithValue("@SellingPrice", Convert.ToDouble(sellingtxt.Text));
                cm.Parameters.AddWithValue("@Cashier", uname);

                cn.Open();
                cm.ExecuteNonQuery();
                cn.Close();
                //DisplayAutoDisappearingMessageBox("This message will disappear in 3 seconds.", "Auto-Disappearing Message");

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }

            cash.LoadCash();
        }

        private void DisplayAutoDisappearingMessageBox(string message, string title)
        {
           
        }

        private void discounttxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            double originalPrice;
            double discountAmount;

            if (double.TryParse(pricetxt.Text, out originalPrice))
            {
                if (discounttxt.Text.EndsWith("%"))
                {
                    string discountValue = discounttxt.Text.TrimEnd('%');
                    if (double.TryParse(discountValue, out double discountPercentage))
                    {
                        double discount = originalPrice * (discountPercentage / 100);
                        double sellingPrice = originalPrice - discount;
                        sellingtxt.Text = sellingPrice.ToString();
                    }
                    else
                    {
                        
                        MessageBox.Show("Invalid discount percentage!");
                    }
                }
                else if (double.TryParse(discounttxt.Text, out discountAmount))
                {
                    double sellingPrice = originalPrice - discountAmount;
                    sellingtxt.Text = sellingPrice.ToString();
                }
                else
                {
                   
                    MessageBox.Show("Invalid discount amount!");
                }
            }
            else
            {
               
                MessageBox.Show("Invalid price!");
            }
        }
    }
}
