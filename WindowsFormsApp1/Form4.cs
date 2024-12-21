using System;
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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        MySqlConnection conn = new MySqlConnection("server=localhost;database=pharmacy;user=root;password=");
        string query = "";

        private void button6_Click(object sender, EventArgs e)
        {
            string nameadmin = textBox1.Text;
            string passadmin = textBox2.Text;


            MySqlCommand comanda = new MySqlCommand("select * from administration where name_administration=@nameadmin and password_administration=@passadmin;", conn);
            comanda.Parameters.AddWithValue("@nameadmin", nameadmin);
            comanda.Parameters.AddWithValue("@passadmin", passadmin);

            try
            {
                if (textBox1.Text.Length > 0 || textBox2.Text.Length > 0)
                {
                    conn.Open();

                    MySqlDataReader reader = comanda.ExecuteReader();

                    if (reader.HasRows) // Проверка, есть ли результаты
                    {
                        Form2 form2 = new Form2();
                        form2.Show();
                        MessageBox.Show("Salom");
                    }
                    else
                    {
                        MessageBox.Show("Неправильное имя или пароль");
                    }

                   
                }
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
