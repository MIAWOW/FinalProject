using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;

namespace Italian_Restaurant
{

    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection(@"Data Source=LAB109PC22\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        public static string userId = "";

        private void button2_Click(object sender, EventArgs e)
        {
            String Name = textBoxUser.Text;
            String Email = textBoxMail.Text;
            String Password = textBoxPass.Text;
            String Address = textBoxAddress.Text;

            try
            {
                string query = "USE restaurant;" +
                               "INSERT INTO Users VALUES (@Name, @Email, @Address, @Password)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Password", Password);
                cmd.Parameters.AddWithValue("@Address", Address);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                sda.Fill(dt);

                string query2 = "USE restaurant;" +
                                "SELECT * " +
                                "FROM Users " +
                                "WHERE Email = @Email";

                SqlCommand cmd2 = new SqlCommand(@query2, conn);
                cmd2.Parameters.AddWithValue("@Email", Email);

                SqlDataAdapter sda2 = new SqlDataAdapter(cmd2);

                DataTable dt2 = new DataTable();
                sda2.Fill(dt2);

                userId = Convert.ToString(dt2.Rows[0]["ID"]);

                this.Hide();
                Form3 home = new Form3();
                home.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }

        }

        private void Login_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form1 login = new Form1();
            login.Show();
        }
    }
}
