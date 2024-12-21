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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        MySqlConnection conn = new MySqlConnection("server=localhost;database=pharmacy;user=root;password=adminroot");
        string query = "";
        string cellvalue = "";
        string cellvaluecolumn = "";



        private void Form2_Load(object sender, EventArgs e)
        {
            query = "select * from medicines;";

            MySqlDataAdapter adapter1 = new MySqlDataAdapter(query, conn);
            DataTable dt1 = new DataTable();
            adapter1.Fill(dt1);

            try
            {
                conn.Open();

                dataGridView1.DataSource = dt1;
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            cellvalue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            cellvaluecolumn = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(cellvaluecolumn);
            query = $"update medicines set name_medicine='{cellvalue}' where id={cellvaluecolumn};";
            try
            {
                conn.Open();

                MySqlCommand comanda = new MySqlCommand(query, conn);
                comanda.ExecuteNonQuery();

                if (comanda.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("success" + comanda.ExecuteNonQuery());
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

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show(cellvalue);
            query = $"DELETE FROM medicines WHERE id={cellvaluecolumn};";

            try
            {
                conn.Open();

                MySqlCommand comanda1 = new MySqlCommand(query, conn);
                comanda1.ExecuteNonQuery();
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

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(cellvalue);
            query = $"update medicines set price={cellvalue}' where id={cellvaluecolumn};";

            try
            {
                conn.Open();

                MySqlCommand comanda = new MySqlCommand(query, conn);
                comanda.ExecuteNonQuery();

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

        private void button6_Click(object sender, EventArgs e)
        {
            query = "select * from medicines;";

            MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            try
            {
                conn.Open();

                dataGridView1.DataSource = dt;
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

        private void button1_Click(object sender, EventArgs e)
        {
            string namemedicine = textBox1.Text;
            string pricemedicine = textBox2.Text;

            try
            {

                conn.Open();

                MySqlCommand comandtoinsertmedisine = new MySqlCommand("INSERT INTO medicines (name_medicine, price) VALUES (@namemedicine, @pricemedicine);", conn);
                comandtoinsertmedisine.Parameters.AddWithValue("@namemedicine", namemedicine);
                comandtoinsertmedisine.Parameters.AddWithValue("@pricemedicine", pricemedicine);

                DialogResult result = MessageBox.Show("are agree to add this medicine?", "!?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                  int countcomand = comandtoinsertmedisine.ExecuteNonQuery();
                    if (countcomand > 0)
                    {
                        MessageBox.Show("the new midicen added to list!");
                    }
                }
                else
                {
                    return;
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

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
