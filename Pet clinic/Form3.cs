using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Pet_clinic
{
    public partial class Form3 : Form
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\MSI\Desktop\Pet clinic\Pet clinic\bin\Debug\Pet clinic1.accdb";
        int clients_ID;

        public Form3(int clientId)
        {
            InitializeComponent();
            clients_ID = clientId;
        }

           decimal total = 0;




        private void Form3_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Skin Infection");
            comboBox1.Items.Add("Ear Infection");
            comboBox1.Items.Add("Eye Infection");
            comboBox1.Items.Add("Fever");
            comboBox1.Items.Add("Parasites");

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = comboBox1.SelectedItem.ToString();
            switch (selected)
            {
                case "Skin Infection":
                    textBox2.Text = "20";
                    break;
                case "Ear Infection":
                    textBox2.Text = "25";
                    break;
                case "Eye Infection":
                    textBox2.Text = "15";
                    break;
                case "Fever":
                    textBox2.Text = "30";
                    break;
                case "Parasites":
                    textBox2.Text = "40";
                    break;
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
           

            if (radioButton2.Checked)
            {
                int days = (int)numericUpDown1.Value;
                decimal pricePerDay = decimal.Parse(textBox1.Text);
                total = days * pricePerDay;
            }
            else if (radioButton1.Checked)
            {
                total = 15; // سعر ثابت للتنظيف
            }
            else if (radioButton3.Checked)
            {
                total = decimal.Parse(textBox2.Text);
            }

            label6.Text = "Total: $" + total.ToString("0.00");

        }

        private void button2_Click(object sender, EventArgs e)
        
           
        {

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                string serviceType = "";

                if (radioButton1.Checked)
                    serviceType = "Cleaning";
                else if (radioButton2.Checked)
                    serviceType = "Boarding";
                else if (radioButton3.Checked)
                    serviceType = comboBox1.Text;

                string insertService = "INSERT INTO services (clientID, servicestype, details, totalprice) VALUES (?, ?, ?, ?)";
                OleDbCommand cmd = new OleDbCommand(insertService, conn);
                cmd.Parameters.AddWithValue("?", clients_ID);  // الربط بالعميل
                cmd.Parameters.AddWithValue("?", serviceType);
                cmd.Parameters.AddWithValue("?", "");  // ممكن تملأ حقل التفاصيل لاحقًا
                cmd.Parameters.AddWithValue("?", total);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Service added successfully.");
            }

            Form4 form4 = new Form4(clients_ID);
            form4.Show();
            this.Hide();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
