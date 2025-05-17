using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Pet_clinic
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\MSI\Desktop\Pet clinic\Pet clinic\bin\Debug\Pet clinic1.accdb";
        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(new string[] { "dog", "cat", "bird", "rabbit", "hamster" });

            
            AutoCompleteStringCollection clientNames = new AutoCompleteStringCollection();

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT DISTINCT ownername FROM clients";
                OleDbCommand cmd = new OleDbCommand(query, conn);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clientNames.Add(reader["ownername"].ToString());
                    }
                }
            }

            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox1.AutoCompleteCustomSource = clientNames;
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            string ownerName = textBox1.Text.Trim();
            comboBox1.Items.Clear();

            if (ownerName.Length == 0)
                return;

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT petname FROM clients WHERE ownername = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", ownerName);

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["petname"].ToString());
                    }
                }
            }
        }

       
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ownerName = textBox1.Text.Trim();
            string petName = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(petName))
                return;

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT pettype FROM clients WHERE ownername = ? AND petname = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("?", ownerName);
                cmd.Parameters.AddWithValue("?", petName);

                var petType = cmd.ExecuteScalar();
                if (petType != null)
                {
                    textBox2.Text = petType.ToString();
                }
                else
                {
                    textBox2.Text = "";
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

            string ownerName = textBox1.Text.Trim();
            string petName = textBox2.Text.Trim();
            string petType = comboBox1.Text.Trim();
            int age = (int)numericUpDown1.Value;

            int clientId = -1;

            

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string checkQuery = "SELECT clients_ID FROM clients WHERE petname = ? AND ownername = ?";
                OleDbCommand checkCmd = new OleDbCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("?", petName);
                checkCmd.Parameters.AddWithValue("?", ownerName);

                var result = checkCmd.ExecuteScalar();

                if (result != null)
                {
                    clientId = Convert.ToInt32(result);
                    MessageBox.Show("Client already exists. Using existing record.");
                }
                else
                {
                  
                    string insertQuery = "INSERT INTO clients (ownername, petname, pettype, age) VALUES (?, ?, ?, ?)";
                    OleDbCommand insertCmd = new OleDbCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("?", ownerName);
                    insertCmd.Parameters.AddWithValue("?", petName);
                    insertCmd.Parameters.AddWithValue("?", petType);
                    insertCmd.Parameters.AddWithValue("?", age);
                    insertCmd.ExecuteNonQuery();

                   
                    insertCmd = new OleDbCommand("SELECT @@IDENTITY", conn);
                    clientId = Convert.ToInt32(insertCmd.ExecuteScalar());
                    MessageBox.Show("New client added.");
                }

                
                Form3 ServiceForm = new Form3(clientId);
                ServiceForm.Show();
                this.Hide();
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
