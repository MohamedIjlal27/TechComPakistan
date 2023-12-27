using Microsoft.Reporting.WinForms;
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
    public partial class Report : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        //string title = "TechComPakistan";
        public Report()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
        }

        private void loadbtn_Click(object sender, EventArgs e)
        {
            if(reportcbm.Text == "Sales Report")
            {
                DateTime fromDate = fromdate.Value;
                DateTime toDate = todate.Value;

                string query = @"SELECT s.PCode,s.TransNo, s.PName, s.Qty, s.OriginalPrice, s.DiscountPrice, 
                             s.SellingPrice, s.Total, s.CID, s.Cashier, s.Date, Cu.Name
                             FROM SalesTable s
                             LEFT JOIN CustomerTable Cu ON s.CID = Cu.ID
                             WHERE s.Date >= @fromdate AND s.Date <= @todate";

                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    cm.Parameters.AddWithValue("@fromdate", fromDate);
                    cm.Parameters.AddWithValue("@todate", toDate);

                    using (SqlDataAdapter DP = new SqlDataAdapter(cm))
                    {
                        DataTable DT = new DataTable();
                        DP.Fill(DT);



                        reportViewer1.LocalReport.DataSources.Clear();

                        ReportDataSource dataSource1 = new ReportDataSource("DataSet1", DT);
                        ReportDataSource dataSource2 = new ReportDataSource("DataSet2", DT);

                        reportViewer1.LocalReport.ReportPath = @"D:\Project\Inventory Managment System (2)\Inventory Managment System\WindowsFormsApp1\SalesReport.rdlc";
                        reportViewer1.LocalReport.DataSources.Add(dataSource1);
                        reportViewer1.LocalReport.DataSources.Add(dataSource2);
                        reportViewer1.RefreshReport();
                    }
                }
            }
            else if (reportcbm.Text=="Customer Report")
            {
                DateTime fromDate = fromdate.Value;
                DateTime toDate = todate.Value;

                string query = @"SELECT *
                                 FROM CustomerTable
                                 WHERE Date >= @fromdate AND Date <= @todate";

                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    cm.Parameters.AddWithValue("@fromdate", fromDate);
                    cm.Parameters.AddWithValue("@todate", toDate);

                    using (SqlDataAdapter DP = new SqlDataAdapter(cm))
                    {
                        DataTable DT = new DataTable();
                        DP.Fill(DT);



                        reportViewer1.LocalReport.DataSources.Clear();

                        ReportDataSource dataSource1 = new ReportDataSource("DataSet1", DT);

                        reportViewer1.LocalReport.ReportPath = @"D:\Project\Inventory Managment System (2)\Inventory Managment System\WindowsFormsApp1\Report1.rdlc";
                        reportViewer1.LocalReport.DataSources.Add(dataSource1);
                        reportViewer1.RefreshReport();
                    }
                }
            }
            else if (reportcbm.Text == "Supplier Report")
            {
                DateTime fromDate = fromdate.Value;
                DateTime toDate = todate.Value;

                string query = @"SELECT * FROM SupplierTable
                                 WHERE Date >= @fromdate AND Date <= @todate";

                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    cm.Parameters.AddWithValue("@fromdate", fromDate);
                    cm.Parameters.AddWithValue("@todate", toDate);

                    using (SqlDataAdapter DP = new SqlDataAdapter(cm))
                    {
                        DataTable DT = new DataTable();
                        DP.Fill(DT);



                        reportViewer1.LocalReport.DataSources.Clear();

                        ReportDataSource dataSource1 = new ReportDataSource("DataSet1", DT);

                        reportViewer1.LocalReport.ReportPath = @"D:\Project\Inventory Managment System (2)\Inventory Managment System\WindowsFormsApp1\Supplier.rdlc";
                        reportViewer1.LocalReport.DataSources.Add(dataSource1);
                        reportViewer1.RefreshReport();
                    }
                }
            }
            else if(reportcbm.Text == "Product Report")
            {
                DateTime fromDate = fromdate.Value;
                DateTime toDate = todate.Value;

                string query = @"SELECT * FROM ProductTable
                                 WHERE Date >= @fromdate AND Date <= @todate";

                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    cm.Parameters.AddWithValue("@fromdate", fromDate);
                    cm.Parameters.AddWithValue("@todate", toDate);

                    using (SqlDataAdapter DP = new SqlDataAdapter(cm))
                    {
                        DataTable DT = new DataTable();
                        DP.Fill(DT);



                        reportViewer1.LocalReport.DataSources.Clear();

                        ReportDataSource dataSource1 = new ReportDataSource("DataSet1", DT);

                        reportViewer1.LocalReport.ReportPath = @"D:\Project\Inventory Managment System (2)\Inventory Managment System\WindowsFormsApp1\Product.rdlc";
                        reportViewer1.LocalReport.DataSources.Add(dataSource1);
                        reportViewer1.RefreshReport();
                    }
                }
            }
        }

        
    }

}
 