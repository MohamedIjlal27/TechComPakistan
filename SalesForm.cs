using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class SalesForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string title = "TechComPakistan";
        Main main;
        string _name = "", _qty = "", _pname = "", _total = "";
        string _price = "",_discountprice="", _sellingprice = "";
        public SalesForm(Main form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            main = form;
            getTranNo();
            LoadCash();
        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            SalesProduct product = new SalesProduct(this);
            product.uname = main.lblusername.Text;
            product.ShowDialog();
        }

        private void btncash_Click(object sender, EventArgs e)
        {
            SalesCustomer cash = new SalesCustomer(this);
            cash.ShowDialog();

        }        

        private void dgvcash_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ColName = dgvcash.Columns[e.ColumnIndex].Name;
            removeitem:
                if (ColName == "Delete")
                {
                    if (MessageBox.Show("Are You sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {

                        dbcon.executeQuery("DELETE FROM SalesTable WHERE SalesID LIKE '" + dgvcash.Rows[e.RowIndex].Cells[1].Value.ToString()+ "'");
                        MessageBox.Show("Item has been successfully deleted", title, MessageBoxButtons.OK, MessageBoxIcon.Question);

                    }                
                }
                else if (ColName == "Increase")
                {
                    int i = chckPqty(dgvcash.Rows[e.RowIndex].Cells[2].Value.ToString());
                
                    if(int.Parse(dgvcash.Rows[e.RowIndex].Cells[4].Value.ToString()) < i)
                    {
                        dbcon.executeQuery("UPDATE SalesTable SET Qty=Qty+" + 1 + " WHERE SalesID LIKE '" + dgvcash.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
                    }
                    else
                    {
                        MessageBox.Show("Remaining Quantity on hand is " + i + "!", "Out Of Stock!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }   
                    
                }
                else if (ColName == "Decrease")
                {
                    if (int.Parse(dgvcash.Rows[e.RowIndex].Cells[4].Value.ToString()) > 1)
                    {
                        dbcon.executeQuery("UPDATE SalesTable SET Qty=Qty-" + 1 + "WHERE SalesID LIKE '" + dgvcash.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
                    }
                    else
                    {
                        ColName = "Delete";
                        goto removeitem;
                    }                   
                }
            

            LoadCash();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            
                try
                {
                    int i = 0;
                    double total = 0;
                    cm = new SqlCommand("SELECT sales.SalesID, sales.PCode, sales.PName, sales.Qty, sales.OriginalPrice, sales.DiscountPrice, sales.SellingPrice, sales.Total, c.Name, sales.Cashier FROM SalesTable as sales LEFT JOIN CustomerTable c ON sales.CID = c.ID WHERE TransNo LIKE @TransactionNo", cn);
                    cm.Parameters.AddWithValue("@TransactionNo", lblTransaction.Text);
                    cn.Open();
                    dr = cm.ExecuteReader();

                    int logoWidth = 100;
                    //int logoHeight = 100;
                    int logoX = 10;
                    int logoY = 10;

                    Font logoFont = new Font("Arial", 12);
                    int detailsX = logoX + logoWidth + 10;
                    int detailsY = logoY + 10;

               




                string shopName = "TechCom Pakistan";
                Font shopNameFont = new Font("Century Gothic", 38, FontStyle.Bold);
                SolidBrush textBrush = new SolidBrush(Color.SkyBlue);

                e.Graphics.DrawString(shopName, shopNameFont, textBrush, detailsX, detailsY);

                Pen underlinePen = new Pen(textBrush.Color);
                int underlineY = detailsY + shopNameFont.Height + 5;

                e.Graphics.DrawLine(underlinePen, detailsX, underlineY, detailsX + e.Graphics.MeasureString(shopName, shopNameFont).Width, underlineY);

                string ShopComment1 = "All the Computer & Accessories, Laptop, IT Solutions, FIber Optical Solution";
                string ShopComment2 = "Security System, Networking & Bussiness Communication";
                Font ShopComment1Font = new Font("Century Gothic", 11, FontStyle.Bold);

                e.Graphics.DrawString(ShopComment1, ShopComment1Font, Brushes.Black, detailsX, detailsY + 80);
                e.Graphics.DrawString(ShopComment2, ShopComment1Font, Brushes.Black, detailsX + 40, detailsY + 100);



                string transactionNumber = lblTransaction.Text;
                string dashline = "---------------------------------------------------------------------------------------------------------------------------------------------";
                e.Graphics.DrawString("Transaction Number: " + transactionNumber, new Font("Arial", 12), Brushes.Black, new PointF(50, 180));
                e.Graphics.DrawString(dashline, new Font("Century Gothic", 12), Brushes.Black, new PointF(50, 220));
                e.Graphics.DrawString("Item ", new Font("Century Gothic", 12), Brushes.Black, new PointF(75, 300));
                e.Graphics.DrawString("Price", new Font("Century Gothic", 12), Brushes.Black, new PointF(300, 300));
                e.Graphics.DrawString("Discount", new Font("Century Gothic", 12), Brushes.Black, new PointF(400, 300));
                e.Graphics.DrawString("Selling Price", new Font("Century Gothic", 12), Brushes.Black, new PointF(500, 300));
                e.Graphics.DrawString("No", new Font("Century Gothic", 12), Brushes.Black, new PointF(650, 300));
                e.Graphics.DrawString("Total", new Font("Century Gothic", 12), Brushes.Black, new PointF(750, 300));
                e.Graphics.DrawString(dashline, new Font("Century Gothic", 12), Brushes.Black, new PointF(50, 320));


                string shopAddress = "Shop No. 44, Shoukat Plaza, G.T. Road, Haripur.";
                string shopContact = "Phone: 0995-614400, Email: techcompakistan@gmail.com";
                Font shopDetailsFont = new Font("Arial", 12);

                int shopDetailsX = 100;
                int shopDetailsY = e.PageBounds.Height - 100;

                e.Graphics.DrawString(shopAddress, shopDetailsFont, Brushes.Black, new PointF(shopDetailsX, shopDetailsY));
                e.Graphics.DrawString(shopContact, shopDetailsFont, Brushes.Black, new PointF(shopDetailsX, shopDetailsY + 20));


                while (dr.Read())
                    {
                        i++;
                        dgvcash.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString(), dr[9].ToString());
                        total += double.Parse(dr[7].ToString());
                        _name = dr["Name"].ToString();
                        _pname = dr["PName"].ToString();
                        _total = dr["Total"].ToString();
                        _qty = dr["Qty"].ToString();
                        _price = dr["OriginalPrice"].ToString();
                        _discountprice = dr["DiscountPrice"].ToString();
                        _sellingprice = dr["SellingPrice"].ToString();

                    int productNameX = 75;
                    int productNameY = 350 + (i - 1) * 50;
                    int priceX = 300;
                    int priceY = 350 + (i - 1) * 50;
                    int discountx = 400;
                    int discounty = 350 + (i - 1) * 50;
                    int sellingpricex = 500;
                    int sellingpricey = 350 + (i - 1) * 50;
                    int quantityX = 650;
                    int quantityY = 350 + (i - 1) * 50;
                    int totalX = 750;
                    int totalY = 350 + (i - 1) * 50;

                    e.Graphics.DrawString("Customer Name: " + _name, new Font("Century Gothic", 12), Brushes.Black, new PointF(50, 200));
                    e.Graphics.DrawString("Date: " + DateTime.Now, new Font("Century Gothic", 11, FontStyle.Regular), Brushes.Black, new Point(600, 180));
                    e.Graphics.DrawString(_pname, new Font("Century Gothic", 12), Brushes.Black, new PointF(productNameX, productNameY));
                    e.Graphics.DrawString(_price, new Font("Century Gothic", 12), Brushes.Black, new PointF(priceX, priceY));
                    e.Graphics.DrawString(_discountprice, new Font("Century Gothic", 12), Brushes.Black, new PointF(discountx, discounty)); 
                    e.Graphics.DrawString(_sellingprice, new Font("Century Gothic", 12), Brushes.Black, new PointF(sellingpricex, sellingpricey));
                    e.Graphics.DrawString(_qty, new Font("Century Gothic", 12), Brushes.Black, new PointF(quantityX, quantityY)); 
                    e.Graphics.DrawString(_total, new Font("Century Gothic", 12), Brushes.Black, new PointF(totalX, totalY));

                }
                dr.Close();
                    cn.Close();

                    int totalLabelX = 100;
                    int totalLabelY = 350 + i * 50;
                    int totalValueX = 750;
                    int totalValueY = 350 + i * 50;
                    e.Graphics.DrawString(dashline, new Font("Century Gothic", 12), Brushes.Black, new PointF(50, 320 + i * 50));
                    e.Graphics.DrawString("Total Amount of Payment:", new Font("Arial", 12), Brushes.Black, new PointF(totalLabelX, totalLabelY));
                    e.Graphics.DrawString(total.ToString("#,##0.00"), new Font("Arial", 12), Brushes.Black, new PointF(totalValueX, totalValueY));
                    e.Graphics.DrawString(dashline, new Font("Century Gothic", 12), Brushes.Black, new PointF(50, 400 + i * 50));

                    LoadCash();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, title);
                }
            

        }

        private void preview_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);

            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;

            printPreviewDialog.ShowDialog();
        }

        private void btncrashout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cash this product?", "Cashing", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    PrintDocument printDocument = new PrintDocument();
                    printDocument.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);

                    PrintDialog printDialog = new PrintDialog();
                    printDialog.Document = printDocument;

                    if (printDialog.ShowDialog() == DialogResult.OK)
                    {
                        printDocument.Print();
                        getTranNo();
                        main.LoadDailySales();

                        using (SqlConnection connection = new SqlConnection(dbcon.connection()))
                        {
                            connection.Open();

                            // Update the CashTable with the current date
                            string updateQuery = $"UPDATE SalesTable SET Date = @Date WHERE SalesID IN (SELECT TOP (@RowCount) SalesID FROM SalesTable ORDER BY SalesID DESC)";
                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@Date", DateTime.Now);
                                updateCommand.Parameters.AddWithValue("@RowCount", dgvcash.Rows.Count);
                                updateCommand.ExecuteNonQuery();
                            }

                            // Update the ProductTable
                            for (int i = 0; i < dgvcash.Rows.Count; i++)
                            {
                                string productCode = dgvcash.Rows[i].Cells[2].Value.ToString();
                                int quantity = int.Parse(dgvcash.Rows[i].Cells[4].Value.ToString());

                                string updateProductQuery = "UPDATE ProductTable SET PQty = PQty - @Quantity WHERE Pcode = @Pcode";
                                using (SqlCommand updateProductCommand = new SqlCommand(updateProductQuery, connection))
                                {
                                    updateProductCommand.Parameters.AddWithValue("@Quantity", quantity);
                                    updateProductCommand.Parameters.AddWithValue("@Pcode", productCode);
                                    updateProductCommand.ExecuteNonQuery();
                                }

                            }

                        }

                    }

                    dgvcash.Rows.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void CashForm_Load(object sender, EventArgs e)
        {
            LoadCash();
        }


        #region Method
        public void getTranNo()
        {
            try
            {
                string sdate = DateTime.Now.ToString("yyyyMMdd");
                int count;
                string transno;

                cn.Open();
                cm = new SqlCommand("SELECT TOP 1 TransNo FROM SalesTable WHERE TransNo LIKE '" + sdate + "%' ORDER BY SalesID DESC", cn);
                dr = cm.ExecuteReader();
                dr.Read();

                if (dr.HasRows)
                {
                    transno = dr[0].ToString();
                    count = int.Parse(transno.Substring(8, 4));
                    lblTransaction.Text = sdate + (count + 1);
                }
                else
                {
                    transno = sdate + "1001";
                    lblTransaction.Text = transno;
                }
                dr.Close();
                cn.Close();

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }

        }

        public void LoadCash()
        {
            try
            {
                int i = 0;
                double total = 0;
                dgvcash.Rows.Clear();
                cm = new SqlCommand("SELECT SalesID, PCode, PName, Qty, OriginalPrice, DiscountPrice, SellingPrice, Total, c.Name, Cashier FROM SalesTable as sales LEFT JOIN CustomerTable c ON sales.CID = c.ID WHERE TransNo LIKE @Transaction", cn);
                cm.Parameters.AddWithValue("@Transaction", lblTransaction.Text);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvcash.Rows.Add(i, dr["SalesID"].ToString(), dr["PCode"].ToString(), dr["PName"].ToString(), dr["Qty"].ToString(), dr["OriginalPrice"].ToString(), dr["DiscountPrice"].ToString(), dr["SellingPrice"].ToString(), dr["Total"].ToString(), dr["Name"].ToString(), dr["Cashier"].ToString());
                    double temp;
                    if (double.TryParse(dr["Total"].ToString(), out temp))
                    {
                        total += temp;
                    }
                }
                dr.Close();
                cn.Close();
                lblTotal.Text = total.ToString("#,##0.00");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        public int chckPqty(string PCode)
        {
            int i = 0;
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT PQty FROM ProductTable WHERE PCode LIKE '" + PCode + "'", cn);
                i = int.Parse(cm.ExecuteScalar().ToString());
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }

            return i;
        }


        #endregion Method
    }
}