using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        MySqlConnection conn = new MySqlConnection("server=localhost;database=pharmacy;user=root;password=adminroot");
        string query = "";

        private void button4_Click(object sender, EventArgs e)
        {
            string combotext1 = comboBox1.Text; //month
            string combotext2 = comboBox2.Text; //day
            string combotext3 = comboBox3.Text; //year

            if (comboBox1.Text.Length > 0 && comboBox2.Text.Length > 0 && comboBox3.Text.Length > 0)
            {
                query = @"SELECT name_costumer, total_amount, data_sell from costumer JOIN sells ON costumer.id_sell=sells.id_sell WHERE DATE(data_sell)=@query1;";
                MySqlCommand commanda = new MySqlCommand(query, conn);

                string query1 = $"{combotext3}-{combotext1}-{combotext2}";

                commanda.Parameters.AddWithValue("@query1", query1);
                MySqlDataAdapter adapter = new MySqlDataAdapter(commanda);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                try
                {
                    conn.Open();
                    dataGridView2.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
