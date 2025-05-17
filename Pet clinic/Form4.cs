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

namespace Pet_clinic
{
    public partial class Form4: Form
    {
        int clients_ID;
        int ID;
        public Form4(int clientId)
{
    InitializeComponent();
    clients_ID = clientId;
}


        private void Form4_Load(object sender, EventArgs e)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\MSI\Desktop\Pet clinic\Pet clinic\bin\Debug\Pet clinic1.accdb";

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                // 1. عرض معلومات العميل
                string clientQuery = "SELECT ownername, petname, pettype, age FROM clients WHERE clients_ID = ?";
                OleDbCommand clientCmd = new OleDbCommand(clientQuery, conn);
                clientCmd.Parameters.AddWithValue("?", clients_ID);

                using (OleDbDataReader reader = clientCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        label1.Text = "owner name:" + reader["ownername"].ToString();
                        label2.Text = "pet name:" + reader["petname"].ToString();
                        label3.Text = "pet type:" + reader["pettype"].ToString();
                        label4.Text = "pet age:" + reader["age"].ToString();
                    }
                }

                // 2. عرض الخدمة المختارة وسعرها
                string serviceQuery = "SELECT servicestype, totalprice FROM services WHERE clientID = ?";
                OleDbCommand serviceCmd = new OleDbCommand(serviceQuery, conn);
                serviceCmd.Parameters.AddWithValue("?", clients_ID);


                using (OleDbDataReader reader = serviceCmd.ExecuteReader())
                {
                    decimal total = 0;

                    while (reader.Read())
                    {
                        Services.Items.Add($"{reader["servicestype"]} - {reader["totalprice"]} JD");


                        total += Convert.ToDecimal(reader["totalprice"]);
                    }

                    label5.Text = "Total price:" + $"Total: {total} JD";
                }
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
