using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Login : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        SqlDataReader dr;
        string title = "TechComPakistan";
        public Login()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
        }

        private void closebtn_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Exit Application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void Loginbtn_Click(object sender, EventArgs e)
        {
            try
            {
                string _name = "";
                string _role = "";
                byte[] _imageData;

                cn.Open();
                cm = new SqlCommand("SELECT Name, Role, PictureBox FROM UserInfo WHERE Name=@Name and Password=@Password", cn);
                cm.Parameters.AddWithValue("@Name", nametxt.Text);
                cm.Parameters.AddWithValue("@Password", passtxt.Text);
                dr = cm.ExecuteReader();

                if (dr.Read())
                {
                    _name = dr["Name"].ToString();
                    _role = dr["Role"].ToString();
                    _imageData = (byte[])dr["PictureBox"];
                    MessageBox.Show("Welcome " + _name, "ACCESS GRANTED", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Insert user log
                   

                    Main main = new Main();
                    main.lblusername.Text = _name;
                    main.lblrole.Text = _role;

                    using (MemoryStream ms = new MemoryStream(_imageData))
                    {
                        main.pic.Image = Image.FromStream(ms);
                    }

                    if (_role == "Administrator")
                    {
                        main.userbtn.Enabled = true;
                        main.salebtn.Enabled = true;
                    }
                    else if (_role == "Cashier")
                    {
                        main.salebtn.Enabled = true;
                    }


                    dr.Close();

                    /*string userName = nametxt.Text;
                    DateTime loginDateTime = DateTime.Now;
                    DateTime logoutDateTime = DateTime.Now;
                    cm = new SqlCommand("INSERT INTO UserLog (UserName, LoginDate) VALUES (@UserName, @LoginDate)", cn);
                    cm.Parameters.AddWithValue("@UserName", userName);
                    cm.Parameters.AddWithValue("@LoginDate", loginDateTime);
                    cm.ExecuteNonQuery();*/

                    cn.Close();
                    this.Hide();
                    main.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password!", "ACCESS DENIED", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                   /* string userName = nametxt.Text;
                    DateTime loginDateTime = DateTime.Now;
                    cm = new SqlCommand("INSERT INTO UserLog (UserName, LoginDate) VALUES (@UserName, @LoginDate)", cn);
                    cm.Parameters.AddWithValue("@UserName", userName);
                    cm.Parameters.AddWithValue("@LoginDate", loginDateTime);
                    cm.ExecuteNonQuery();*/
                }
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
                cn.Close();
            }
        }



        private void forgetbtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please Contact Your Administartor!", "FORGET PASSWORD", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void exit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void nametxt_Leave(object sender, EventArgs e)
        {
            passtxt.Focus();
        }
    }
}
