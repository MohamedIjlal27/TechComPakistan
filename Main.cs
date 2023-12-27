using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
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
    public partial class Main : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        public Main()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            LoadDailySales();


        }

        

        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private void dashboardbtn_Click(object sender, EventArgs e)
        {
            openChildForm(new Dashbord());
        }

        private void Customerbtn_Click(object sender, EventArgs e)
        {
            openChildForm(new CustomerForm());
        }

        private void userbtn_Click(object sender, EventArgs e)
        {
            openChildForm(new User());
        }

        private void productbtn_Click(object sender, EventArgs e)
        {
            openChildForm(new Product());
        }

        private void cashbtn_Click(object sender, EventArgs e)
        {
            openChildForm(new SalesForm(this));
        }        

        private void Logoutbtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Logout Application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                   
                    


                    cn.Open();
                    cm.ExecuteNonQuery();
                    cn.Close();

                    Login login = new Login();
                    this.Hide();
                    login.ShowDialog();
                   

                }
                catch (Exception ex)
                {
                    cn.Open();
                    MessageBox.Show(ex.Message, "Error");
                }
            }


        }

        private void exit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }       

        private void Main_Load(object sender, EventArgs e)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!progress.IsDisposed && progress.IsHandleCreated)
            {
                progress.Invoke((MethodInvoker)delegate
                {
                    if (!progress.IsDisposed)
                    {
                        progress.Text = DateTime.Now.ToString("hh:mm:ss");
                        progress.Value = DateTime.Now.Second;
                    }
                });
            }
        }




        private void guna2Button1_Click(object sender, EventArgs e)
        {
            openChildForm(new Supplier());
        }




        #region Mehod
        private Form activeform = null;
        public void openChildForm(Form childForm)
        {
            if (activeform != null)
                activeform.Close();
            activeform = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            lblTitle.Text = childForm.Text;
            panelchile.Controls.Add(childForm);
            panelchile.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        public void LoadDailySales()
        {
            string sdate = DateTime.Now.ToString("yyyyMMdd");
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT ISNULL(SUM(Total), 0) AS Total FROM SalesTable WHERE TransNo LIKE'" + sdate + "%'", cn);
                lblDSales.Text = double.Parse(cm.ExecuteScalar().ToString()).ToString("#,##0.00");
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }

        }
        #endregion Method

        private void mgreportsbtn_Click(object sender, EventArgs e)
        {
            openChildForm(new Report());
        }
    }
}
