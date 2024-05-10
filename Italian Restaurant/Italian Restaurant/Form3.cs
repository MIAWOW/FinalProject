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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            DisplayData("all");
        }

        public static string userId = "";
        public static string orderId = "";

        private void DisplayData(String type)
        {
            try
            {
                
                using (SqlConnection conn = new SqlConnection(@"Data Source=LAB109PC22\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
                {
                    String query = "USE restaurant; ";
                    if (type == "all")
                    {
                        query += "SELECT Product_ID, Picture, Name, Description, Price FROM Products";
                    }
                    else
                    {
                        query += "SELECT Product_ID, Picture, Name, Description, Price FROM Products WHERE Type = @Type";
                    }

                    SqlCommand cmd = new SqlCommand(query, conn);
                    if (type != "all")
                    {
                        cmd.Parameters.AddWithValue("@Type", type);
                    }

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();

                        sda.Fill(dt);

                        flowLayoutPanel1.Controls.Clear();

                        foreach (DataRow row in dt.Rows)
                        {

                            String productId = row["Product_ID"].ToString();
                            String imagePath = row["Picture"].ToString();
                            String name = row["Name"].ToString();
                            String desc = row["Description"].ToString();

                            Image image = Image.FromFile("H:\\Italian Restaurant\\Italian Restaurant\\Pictures\\" + imagePath);


                            PictureBox pictureBox = new PictureBox();
                            pictureBox.Image = image;
                            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                            pictureBox.Width = 70;
                            pictureBox.Height = 70;

                            Label labelName = new Label();
                            labelName.Width = 80;
                            labelName.Text = name;
                            labelName.Height = (int)Math.Ceiling(labelName.CreateGraphics().MeasureString(desc, labelName.Font, labelName.Width).Height);

                            Label labelDesc = new Label();
                            labelDesc.AutoSize = false;
                            labelDesc.Width = 210;
                            labelDesc.Text = desc;
                            labelDesc.Height = (int)Math.Ceiling(labelDesc.CreateGraphics().MeasureString(desc, labelDesc.Font, labelDesc.Width).Height);

                            Button btnQuery = new Button();
                            btnQuery.Text = "+";
                            btnQuery.Tag = productId;

                            btnQuery.Click += BtnQuery_Click;

                            flowLayoutPanel1.Controls.Add(pictureBox);
                            flowLayoutPanel1.Controls.Add(labelName);
                            flowLayoutPanel1.Controls.Add(labelDesc);
                            flowLayoutPanel1.Controls.Add(btnQuery);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Order_Click(object sender, EventArgs e)
        {
            Form5 order = new Form5();
            this.Hide();
            order.Show();
        }

        private void labelPizza_Click(object sender, EventArgs e)
        {
            DisplayData("pizza");
        }

        private void labelPasta_Click(object sender, EventArgs e)
        {
            DisplayData("pasta");
        }

        private void labelDesserts_Click(object sender, EventArgs e)
        {
            DisplayData("dessert");
        }

        private void labelDrinks_Click(object sender, EventArgs e)
        {
            DisplayData("salad");
        }

        private void labelAll_Click(object sender, EventArgs e)
        {
            DisplayData("all");
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn.Tag != null)
            {
                String productId = Convert.ToString(btn.Tag);

                SqlConnection conn = new SqlConnection(@"Data Source=LAB109PC22\SQLEXPRESS;Initial Catalog=restaurant; Integrated Security=True;");

                String query1 = "USE restaurant;" +
                                "SELECT * " +
                                "FROM Orders " +
                                "WHERE User_ID = @User_ID";
                SqlCommand cmd1 = new SqlCommand(query1, conn);
                cmd1.Parameters.AddWithValue("@User_ID", userId);

                SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);

                DataTable dt1 = new DataTable();
                sda1.Fill(dt1);

                orderId = "";

                if (dt1.Rows.Count == 0)
                {
                    String ins_query = "USE restaurant;" +
                                       "INSERT INTO Orders VALUES (@User_ID, @Date)";
                    SqlCommand cmd = new SqlCommand(ins_query, conn);
                    cmd.Parameters.AddWithValue("@User_ID", userId);
                    cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    String sel_query = "USE restaurant;" +
                                       "SELECT TOP 1 * " +
                                       "FROM Orders " +
                                       "ORDER BY Order_ID";
                    SqlCommand sel_cmd = new SqlCommand(@sel_query, conn);
                    SqlDataAdapter sel_sda = new SqlDataAdapter(sel_cmd);
                    DataTable sel_dt = new DataTable();
                    sel_sda.Fill(sel_dt);

                    orderId = Convert.ToString(sel_dt.Rows[0]["Order_ID"]); 
                    
                } 
                else
                {
                    orderId = Convert.ToString(dt1.Rows[0]["Order_ID"]);
                }

                // Order_ID and Product_ID are done

                String qua_query = "USE restaurant;" +
                                   "SELECT * " +
                                   "FROM OrderProducts " +
                                   "WHERE Order_ID = @Order_ID AND Product_ID = @Product_ID";
                SqlCommand qua_cmd = new SqlCommand(qua_query, conn);
                qua_cmd.Parameters.AddWithValue("@Order_ID", orderId);
                qua_cmd.Parameters.AddWithValue("@Product_ID", productId);

                SqlDataAdapter qua_sda = new SqlDataAdapter(qua_cmd);

                DataTable qua_dt = new DataTable();
                qua_sda.Fill(qua_dt);

                if (qua_dt.Rows.Count == 0)
                {
                    String ins_query2 = "USE restaurant;" +
                                        "INSERT INTO OrderProducts VALUES (@Order_ID, @Product_ID, 0)";
                    SqlCommand ins_cmd = new SqlCommand(ins_query2, conn);
                    ins_cmd.Parameters.AddWithValue("@Order_ID", orderId);
                    ins_cmd.Parameters.AddWithValue("@Product_ID", productId);

                    SqlDataAdapter ins_sda = new SqlDataAdapter(ins_cmd);
                    DataTable ins_dt = new DataTable();
                    ins_sda.Fill(ins_dt);
                }

                String query2 = "USE restaurant;" +
                                "UPDATE OrderProducts " +
                                "SET Quantity = Quantity + 1 " +
                                "WHERE Order_ID = @Order_ID AND Product_ID = @Product_ID";

                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.Parameters.AddWithValue("@Order_ID", orderId);
                cmd2.Parameters.AddWithValue("@Product_ID", productId);
                SqlDataAdapter sda2 = new SqlDataAdapter(cmd2);
                DataTable dt2 = new DataTable();
                sda2.Fill(dt2);
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            userId = Form2.userId;
            if (userId == "")
            {
                userId = Form1.userId;
            }
            if (userId == "")
            {
                userId = Form4.userId;
            }
            if (userId == "")
            {
                userId = Form5.userId;
            }
        }

        private void labelProfile_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form4 edit = new Form4();
            edit.Show();
        }

        private void labelap_Click(object sender, EventArgs e)
        {
            DisplayData("appetizer");
        }

        private void labelmain_Click(object sender, EventArgs e)
        {
            DisplayData("main dish");
        }
    }
}
