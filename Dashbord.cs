using DashboardApp.Models;
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
    public partial class Dashbord : Form
    {
        private Dashboard model;

        private Button currentButton;
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        public Dashbord()
        {
            InitializeComponent();
            //Default - Last 7 days
            dtpStartDate.Value = DateTime.Today.AddDays(-7);
            dtpendDate.Value = DateTime.Now;
            btn7days.Select();

            model = new Dashboard();
            LoadData();
        }

        private void LoadData()
        {
            var refreshData = model.LoadData(dtpStartDate.Value, dtpendDate.Value);
            if (refreshData == true)
            {
                lblNumOrders.Text = model.NumOrders.ToString();
                lblTotalRevenue.Text = "$" + model.TotalRevenue.ToString();
                lblTotalProfit.Text = "$" + model.TotalProfit.ToString();

                lblNumCustomers.Text = model.NumCustomers.ToString();
                lblNumSuppliers.Text = model.NumSuppliers.ToString();
                lblNumProducts.Text = model.NumProducts.ToString();

                chartGrossRevenue.DataSource = model.GrossRevenueList;
                chartGrossRevenue.Series[0].XValueMember = "Date";
                chartGrossRevenue.Series[0].YValueMembers = "TotalAmount";
                chartGrossRevenue.DataBind();

                chartTopProducts.DataSource = model.TopProductsList;
                chartTopProducts.Series[0].XValueMember = "Key";
                chartTopProducts.Series[0].YValueMembers = "Value";
                chartTopProducts.DataBind();

                dgvUnderstock.DataSource = model.UnderstockList;
                dgvUnderstock.Columns[0].HeaderText = "Item";
                dgvUnderstock.Columns[1].HeaderText = "Units";
                Console.WriteLine("Loaded view :)");
            }
            else Console.WriteLine("View not loaded, same query");
        }

        private void SetDateMenuButtonUI(object button)
        {
            var btn = (Button)button;
            btn.BackColor = btnlast30days.FlatAppearance.BorderColor;
            btn.ForeColor = Color.White;
            if (currentButton != null && currentButton != btn)
            {
                currentButton.BackColor = this.BackColor;
                currentButton.ForeColor = Color.FromArgb(124, 141, 181);
            }
            currentButton = btn;

            if (currentButton == btncustomedate)
            {
                dtpStartDate.Enabled = true;
                dtpendDate.Enabled = true;
                btnok.Visible = true;
                btnok.Enabled = true;
                lblstartdate.Cursor = Cursors.Hand;
                lblenddate.Cursor = Cursors.Hand;
            }
            else
            {
                dtpStartDate.Enabled = false;
                dtpendDate.Enabled = false;
                btnok.Visible = false;
                lblstartdate.Cursor = Cursors.Default;
                lblenddate.Cursor = Cursors.Default;
            }
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btncustomedate_Click(object sender, EventArgs e)
        {
            SetDateMenuButtonUI(sender);
        }

        private void btntoday_Click(object sender, EventArgs e)
        {
            dtpStartDate.Value = DateTime.Today;
            dtpendDate.Value = DateTime.Now;
            LoadData();
            SetDateMenuButtonUI(sender);
        }

        private void btn7days_Click(object sender, EventArgs e)
        {
            dtpStartDate.Value = DateTime.Today.AddDays(-7);
            dtpendDate.Value = DateTime.Now;
            LoadData();
            SetDateMenuButtonUI(sender);
        }

        private void btnlast30days_Click(object sender, EventArgs e)
        {
            dtpStartDate.Value = DateTime.Today.AddDays(-30);
            dtpendDate.Value = DateTime.Now;
            LoadData();
            SetDateMenuButtonUI(sender);
        }

        private void btnthisMonth_Click(object sender, EventArgs e)
        {
            dtpStartDate.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpendDate.Value = DateTime.Now;
            LoadData();
            SetDateMenuButtonUI(sender);
        }

        private void lblstartdate_Click(object sender, EventArgs e)
        {
            if (currentButton == btncustomedate)
            {
                dtpStartDate.Select();
                SendKeys.Send("%{DOWN}");
            }
        }

        private void lblenddate_Click(object sender, EventArgs e)
        {
            if (currentButton == btncustomedate)
            {
                dtpendDate.Select();
                SendKeys.Send("%{DOWN}");
            }
        }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            lblstartdate.Text = dtpStartDate.Text;
        }

        private void dtpendDate_ValueChanged(object sender, EventArgs e)
        {
            lblenddate.Text = dtpendDate.Text;
        }

        private void dashbord2_Load(object sender, EventArgs e)
        {
            lblstartdate.Text = dtpStartDate.Text;
            lblenddate.Text = dtpendDate.Text;
            dgvUnderstock.Columns[1].Width = 50;
        }

       
      
    }
}
