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

namespace Italian_Restaurant
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        public static string userId = "";

        private void button1_Click(object sender, EventArgs e)
        {
            String Name = textBoxName.Text;
            String Password = textBoxPassword.Text;
            String Address = textBoxAddress.Text;

            SqlConnection conn = new SqlConnection(@"Data Source=LAB109PC22\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            try
            {
                String query = "USE restaurant;";
                if (Name != "")
                {
                    query += " UPDATE Users SET Name = @Name WHERE ID = @User_ID;";
                }
                if (Password != "")
                {
                    query += " UPDATE Users SET Password = @Password WHERE ID = @User_ID;";
                }
                if (Address != "")
                {
                    query += " UPDATE Users SET Address = @Address WHERE ID = @User_ID;";
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                if (Name != "")
                {
                    cmd.Parameters.AddWithValue("@Name", Name);
                }
                if (Password != "")
                {
                    cmd.Parameters.AddWithValue("@Password", Password);
                }
                if (Address != "")
                {
                    cmd.Parameters.AddWithValue("@Address", Address);
                }
                cmd.Parameters.AddWithValue("@User_ID", userId);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                sda.Fill(dt);

                MessageBox.Show("Your information has been updated!", "Updated Info");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            userId = Form3.userId;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 menu = new Form3();
            menu.Show();
        }
    }
}
