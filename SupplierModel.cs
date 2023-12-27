using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class SupplierModel : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string title = "TechComPakistan";

        bool check = false;

        Supplier supplier;

        public SupplierModel(Supplier form)
        {
            InitializeComponent();
            LoadProduct();
            cn = new SqlConnection(dbcon.connection());
            supplier = form;
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
                    if (MessageBox.Show("Are you sure want to register this Supplier?", "Supplier Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("INSERT INTO SupplierTable(Pcode, ProductName, SupplierName, Quantity, Address, Phone, Email, Date) VALUES(@Pcode, @ProductName, @SupplierName, @Quantity, @Address, @Phone, @Email, @Date)", cn);
                        cm.Parameters.AddWithValue("@Pcode", pcodecmbo.Text);
                        cm.Parameters.AddWithValue("@ProductName", producttxt.Text);
                        cm.Parameters.AddWithValue("@SupplierName", suppliertxt.Text);
                        cm.Parameters.AddWithValue("@Quantity", qtytxt.Text);
                        cm.Parameters.AddWithValue("@Address", addresstxt.Text);
                        cm.Parameters.AddWithValue("@Phone", phonetxt.Text);
                        cm.Parameters.AddWithValue("@Email", emailtxt.Text);
                        cm.Parameters.AddWithValue("@Date", datetime.Value);



                        cn.Open();
                        cm.ExecuteNonQuery();


                        cn.Close();
                        MessageBox.Show("Supplier has been registered successfully", title);
                        clear();
                        supplier.LoadSupplier();
                        this.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
                cn.Close();
            }
        }

        private void Updatebtn_Click(object sender, EventArgs e)
        {
            try
            {
                checkfield();
                if (check)
                {
                    if (MessageBox.Show("Are you sure want to Update this Data?", "Supplier Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("UPDATE SupplierTable SET Pcode=@Pcode, ProductName=@ProductName, SupplierName=@SupplierName, Quantity=@Quantity, Address=@Address, Phone=@Phone, Email=@Email, Date=@Date WHERE SID=@SID", cn);
                        cm.Parameters.AddWithValue("@SID", lblsid.Text);
                        cm.Parameters.AddWithValue("@Pcode", pcodecmbo.Text);
                        cm.Parameters.AddWithValue("@ProductName", producttxt.Text);
                        cm.Parameters.AddWithValue("@SupplierName", suppliertxt.Text);
                        cm.Parameters.AddWithValue("@Quantity", qtytxt.Text);
                        cm.Parameters.AddWithValue("@Address", addresstxt.Text);
                        cm.Parameters.AddWithValue("@Phone", phonetxt.Text);
                        cm.Parameters.AddWithValue("@Email", emailtxt.Text);
                        cm.Parameters.AddWithValue("@Date", datetime.Value);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Supplier data has been updated successfully", title);
                        clear();
                        supplier.LoadSupplier();
                        this.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
                cn.Close();
            }
        }

        private void cancelbtn_Click(object sender, EventArgs e)
        {
            clear();
        }

        #region Method
        public void clear()
        {
            pcodecmbo.SelectedIndex=0;
            suppliertxt.Clear();
            producttxt.Clear();
            qtytxt.Clear();
            addresstxt.Clear();
            phonetxt.Clear();
            emailtxt.Clear();
        }

        public void checkfield()
        {
            if (string.IsNullOrEmpty(qtytxt.Text) || string.IsNullOrEmpty(addresstxt.Text) || string.IsNullOrEmpty(phonetxt.Text) || string.IsNullOrEmpty(emailtxt.Text))
            {
                MessageBox.Show("Fill all the required data fields!", "WARNING");
                check = false;
            }
            else
            {
                check = true;
            }
        }

        public void LoadProduct()
        {
            SqlConnection cn = new SqlConnection("Data Source=DESKTOP-SD2A8O9;Initial Catalog=TechComPakistan;Integrated Security=True");

            int i = 0;
            dgvsupplier.Rows.Clear();
            cm = new SqlCommand("SELECT Pcode, PName, PCategory, PPrice FROM ProductTable WHERE CONCAT(PName, PCategory, PPrice)Like'%" + searchtxt.Text + "%' AND PQty > " + 0 + "", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvsupplier.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString());
            }
            dr.Close();
            cn.Close();

        }
        #endregion Method

        private void pcodecmbo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=DESKTOP-SD2A8O9;Initial Catalog=TechComPakistan;Integrated Security=True";
            string selectedProductCode = pcodecmbo.SelectedItem.ToString();
            string query = "SELECT PName FROM ProductTable WHERE Pcode = @Pcode";

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

                        producttxt.Text = name;
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void SupplierModel_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=DESKTOP-SD2A8O9;Initial Catalog=TechComPakistan;Integrated Security=True";
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
                        pcodecmbo.Items.Add(productCode);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
