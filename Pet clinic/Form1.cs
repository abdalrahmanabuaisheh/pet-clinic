using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Pet_clinic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\MSI\Desktop\Pet clinic\Pet clinic\bin\Debug\Pet clinic1.accdb";


        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                string query = "SELECT * FROM [Users] WHERE Username = ? AND Password = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", username);
                cmd.Parameters.AddWithValue("?", password);

                conn.Open();
                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    int userId = Convert.ToInt32(reader["ID"]);

                    MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Form2 welcomeForm = new Form2();
                    welcomeForm.Show();
                  
                    this.Hide();
                }
                else
                {
                    label1.Text = "Invalid username or password";
                    label1.ForeColor = Color.Red;

                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
