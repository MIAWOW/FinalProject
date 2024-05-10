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
    public partial class Form5 : Form
    {
        public Form5()
        {
            userId = Form3.userId;
            InitializeComponent();
            DisplayData();
        }

        public static string userId = "";
        private void DisplayData()
        {
            try
            {

                using (SqlConnection conn = new SqlConnection(@"Data Source=LAB109PC22\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    String get_order_q = "USE restaurant; " +
                                         "SELECT * " +
                                         "FROM Orders " +
                                         "WHERE User_ID = @User_ID";
                    
                    SqlCommand c1 = new SqlCommand(get_order_q, conn);
                    c1.Parameters.AddWithValue("@User_ID", userId);

                    SqlDataAdapter s1 = new SqlDataAdapter(c1);
                    DataTable d1 = new DataTable();
                    s1.Fill(d1);

                    String orderId = Convert.ToString(d1.Rows[0]["Order_ID"]);

                    String query = "USE restaurant; " +
                                   "SELECT p1.Picture, p1.Name, op.Quantity " +
                                   "FROM Products p1 " +
                                   "JOIN OrderProducts op ON p1.Product_ID = op.Product_ID " +
                                   "WHERE p1.Product_ID IN (SELECT op1.Product_ID " +
                                                           "FROM OrderProducts op1 " +
                                                           "WHERE Order_ID = @Order_ID)";
                    
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Order_ID", orderId);

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();

                        sda.Fill(dt);

                        flowLayoutPanel1.Controls.Clear();

                        foreach (DataRow row in dt.Rows)
                        {

                            String imagePath = row["Picture"].ToString();
                            String name = row["Name"].ToString();
                            String quantity = row["Quantity"].ToString();

                            Image image = Image.FromFile("H:\\Italian Restaurant\\Italian Restaurant\\Pictures\\" + imagePath);

                            PictureBox pictureBox = new PictureBox();
                            pictureBox.Image = image;
                            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                            pictureBox.Width = 60;
                            pictureBox.Height = 60;

                            Label labelName = new Label();
                            labelName.Text = name;

                            Label labelQuantity = new Label();
                            labelQuantity.Text = quantity;

                            flowLayoutPanel1.Controls.Add(pictureBox);
                            flowLayoutPanel1.Controls.Add(labelName);
                            flowLayoutPanel1.Controls.Add(labelQuantity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form3 menu = new Form3();
            this.Hide();
            menu.Show();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            userId = Form3.userId;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=LAB109PC22\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            // смятаме цена
            String query = "USE restaurant; " +
                           "SELECT u.Name, u.Address, SUM(p.Price * op.Quantity) AS TotalPrice " +
                           "FROM Orders o " +
                           "    JOIN OrderProducts op ON o.Order_ID = op.Order_ID " +
                           "    JOIN Products p ON op.Product_ID = p.Product_ID " +
                           "    JOIN Users u ON u.ID = @User_ID " +
                           "GROUP BY o.User_ID, u.Name, u.Address";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@User_ID", userId);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            String Name = dt.Rows[0]["Name"].ToString();
            String Address = dt.Rows[0]["Address"].ToString();
            String totalPrice = dt.Rows[0]["TotalPrice"].ToString();

            // премахваме поръчката от базата
            query = "USE restaurant; " +
                    "DELETE FROM OrderProducts WHERE Order_ID IN (SELECT Order_ID FROM Orders WHERE User_ID = @User_ID);" +
                    "DELETE FROM Orders WHERE User_ID = @User_ID;";

            SqlCommand cmd2 = new SqlCommand(query, conn);
            cmd2.Parameters.AddWithValue("@User_ID", userId);

            SqlDataAdapter sda2 = new SqlDataAdapter(cmd2);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);

            // принтираме информация
            MessageBox.Show(Name + ", your order has been approved. It will be delivered to your address " + Address + " within 30 minutes. Total Price: " + totalPrice, "Order Info");

            Form3 menu = new Form3();
            this.Hide();
            menu.Show();
        }
    }
}
