using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class UserModule : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dbcon = new DBConnect();
        string title = "TechComPakistan";

        bool check = false;
        User userform;
        public UserModule(User user)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.connection());
            userform = user;
            rolecmb.SelectedIndex = 1;
        }

        private void savebtn_Click(object sender, EventArgs e)
        {
            try
            {
                checkfield();
                if (check)
                {
                    if (MessageBox.Show("Are you sure you want to register this user?", "User Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        byte[] imageData = ImageToByteArray(picbox.Image);
                        using (SqlCommand cm = new SqlCommand("INSERT INTO UserInfo(Name, Address, Phone, Role, DOB, Password, Date, PictureBox) VALUES(@Name, @Address, @Phone, @Role, @DOB, @Password, @Date, @PictureBox)", cn))
                        {
                            cm.Parameters.AddWithValue("@Name", nametxt.Text);
                            cm.Parameters.AddWithValue("@Address", addresstxt.Text);
                            cm.Parameters.AddWithValue("@Phone", phonetxt.Text);
                            cm.Parameters.AddWithValue("@Role", rolecmb.Text);
                            cm.Parameters.AddWithValue("@DOB", dobdt.Value);
                            cm.Parameters.AddWithValue("@Password", txtpassword.Text);
                            cm.Parameters.AddWithValue("@Date", datetime.Value);
                            cm.Parameters.AddWithValue("@PictureBox", imageData);

                            cn.Open();
                            cm.ExecuteNonQuery();
                        }

                        MessageBox.Show("User has been registered successfully", title);
                        clear();
                        userform.LoadUser();
                        this.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
            finally
            {
                if (cn.State != ConnectionState.Closed)
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
                    if (MessageBox.Show("Are you sure you want to update this record?", "Edit Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        
                        using (SqlCommand cm = new SqlCommand("UPDATE UserInfo SET Name=@Name, Address=@Address, Phone=@Phone, Role=@Role, DOB=@DOB, Password=@Password, Date=@Date WHERE ID=@ID", cn))
                        {
                            cm.Parameters.AddWithValue("@ID", lbluid.Text);
                            cm.Parameters.AddWithValue("@Name", nametxt.Text);
                            cm.Parameters.AddWithValue("@Address", addresstxt.Text);
                            cm.Parameters.AddWithValue("@Phone", phonetxt.Text);
                            cm.Parameters.AddWithValue("@Role", rolecmb.Text);
                            cm.Parameters.AddWithValue("@DOB", dobdt.Value);
                            cm.Parameters.AddWithValue("@Password", txtpassword.Text);
                            cm.Parameters.AddWithValue("@Date", datetime.Value);

                            cn.Open();
                            cm.ExecuteNonQuery();
                        }

                        MessageBox.Show("Record has been successfully updated", title);
                        clear();
                        userform.LoadUser();
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
        }

        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
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

        private void rolecmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(rolecmb.Text== "Employee")
            {
                this.Height = 453 - 27;
                lblPass.Visible = false;
                txtpassword.Visible = false;
            }
            else
            {
                this.Height = 453;
                lblPass.Visible = true;
                txtpassword.Visible = true; 
            }
        }        

        private void picbox_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedImagePath = openFileDialog.FileName;
                // Perform further operations with the selected image path, such as displaying it in the PictureBox or saving it to the database.
                picbox.Image = Image.FromFile(selectedImagePath);
            }
        }

        #region Method
        public void clear()
        {
            nametxt.Clear();
            addresstxt.Clear();
            phonetxt.Clear();
            txtpassword.Clear();
            rolecmb.SelectedIndex = 0;
            dobdt.Value = DateTime.Now;
            picbox.Image = null;
        }

        public void checkfield()
        {
            if (nametxt.Text == "" | addresstxt.Text == " ")
            {
                MessageBox.Show("Fill the required Data Field!, WARNING");
                return;
            }

            if (checkage(dobdt.Value) < 10)
            {
                MessageBox.Show("User is child worker!. Under 18 year", "Warning");
                return;
            }

            check = true;
        }

        private int checkage(DateTime dateofbirth)
        {
            int age = DateTime.Now.Year - dateofbirth.Year;
            if (DateTime.Now.DayOfYear < dateofbirth.DayOfYear)
                age = age - 1;
            return age;


        }

        #endregion Method
    }
}
