using System;
using System.Data;
using System.Data.SqlClient;

namespace Italian_Restaurant
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection(@"Data Source=LAB109PC22\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public static string userId = "";

        private void button1_Click(object sender, EventArgs e)
        {
            String Email = textBoxEmail.Text;
            String Password = textBoxPassword.Text;

            try
            {

                string query = "USE restaurant;" +
                               "SELECT * " +
                               "FROM Users " +
                               "WHERE Email = @Email AND Password = @Password";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Password", Password);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count == 1)
                {
                    Form3 home = new Form3();
                    userId = Convert.ToString(dt.Rows[0]["ID"]);
                    home.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid Login Details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxEmail.Clear();
                    textBoxPassword.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form2 signup = new Form2();
            signup.Show();
        }
    }
}