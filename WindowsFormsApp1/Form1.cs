using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Protobuf.Compiler;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Mysqlx.Resultset;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<Product> products = new List<Product>();

        MySqlConnection conn = new MySqlConnection("server=localhost;database=pharmacy;user=root;password=adminroot");
        string query = "";

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                textBox1.Clear();
            }
            query = "select * from medisines;";
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string search = textBox1.Text;
            query = $"select * from medisines where name_medisine like '%{search}%';";
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            try
            {
                conn.Open();

                dataGridView1.DataSource = dataTable;
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

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            var totalPrice = products.Sum(p => p.Price);

            try
            {

                conn.Open();

                var customerId = GetCustomerId(textBox3.Text);

                MySqlCommand cmd = new MySqlCommand("insert into sells (total_amount, id_customer)values (@totalValue,@id_customer)", conn);
                cmd.Parameters.AddWithValue("@totalValue", totalPrice);
                cmd.Parameters.AddWithValue("@id_customer", customerId);
                cmd.ExecuteNonQuery();
                var id_sell = cmd.LastInsertedId;

                foreach (var item in products)
                {
                    MessageBox.Show($"{item}");

                    MySqlCommand cmd2 = new MySqlCommand($"INSERT INTO orderitems(id_sell, id_medicine, quatity, price) VALUES (@idSel, @idMed, @quantity,@price );", conn);
                    cmd2.Parameters.AddWithValue("@idSel", id_sell);
                    cmd2.Parameters.AddWithValue("@idMed", item.Id);
                    cmd2.Parameters.AddWithValue("@quantity", item.Quantity);
                    cmd2.Parameters.AddWithValue("@price", item.Price);
                    cmd2.ExecuteNonQuery();
                    MessageBox.Show("3work");
                }

                MessageBox.Show("success!");
                textBox3.Clear();
                //panel1.Visible = false;
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


        private int GetCustomerId(string CustomerName)
        {
            MySqlCommand TestNameCmd = new MySqlCommand($"SELECT * FROM customers WHERE name_customer='{CustomerName}';", conn);
            var reader = TestNameCmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                var CustomerId = reader.GetInt32(0);
                reader.Close();
                return CustomerId;
            }
            else
            {
                reader.Close();
                MySqlCommand InsertCustomerCmd = new MySqlCommand($"insert into customers (name_customer) values(@name);", conn);
                InsertCustomerCmd.Parameters.AddWithValue("name", CustomerName);
                InsertCustomerCmd.ExecuteNonQuery();
                return (int)InsertCustomerCmd.LastInsertedId;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
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
            //  panel1.Visible = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox4.Text.Length <= 0)
            {
                MessageBox.Show("write id medicine!");
                return;
            }

            string selectinid = textBox4.Text;

            conn.Open();

            string query = $"SELECT * FROM medicines WHERE id = {selectinid}";

            MySqlCommand TestNameCmd = new MySqlCommand(query, conn);
            var reader = TestNameCmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                var CustomerId = reader.GetInt32(0);

                var medecineId = reader.GetInt32(0);
                var price = reader.GetDouble(2);

                if (products.FirstOrDefault(p => p.Id == medecineId) != null)
                {
                    var product = products.FirstOrDefault(p => p.Id == medecineId);

                    product.Quantity++;
                    product.Price = product.Quantity * price;
                }
                else
                {
                    products.Add(new Product()
                    {
                        Id = medecineId,
                        Name = reader.GetString(1),
                        Price = price,
                        Quantity = 1
                    });
                }

                reader.Close();

            }

            conn.Close();

            var list = new BindingList<Product>(products);
            dataGridView3.DataSource = list;

            //MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
            //DataTable newData = new DataTable();
            //adapter.Fill(newData);

            //try
            //{
            //    conn.Open();

            //    if (dataGridView3.DataSource != null)
            //    {
            //        DataTable existingData = (DataTable)dataGridView3.DataSource;
            //        existingData.Merge(newData);
            //    }
            //    else
            //    {
            //        dataGridView3.DataSource = newData; // Если данных нет, просто присваиваем новые
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error: " + ex.Message);
            //}
            //finally
            //{
            //    conn.Close();
            //    textBox4.Clear();
            //}
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Получаем значение из ячейки
            string cellValue = dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            // Отображаем значение в MessageBox
            MessageBox.Show($"Значение выбранной ячейки: {cellValue}");

        }
        bool TestTrueOrFalse = true;
        private void button6_Click(object sender, EventArgs e)
        {
            if (TestTrueOrFalse)
            {
                panel2.Visible = true;
                TestTrueOrFalse = false;
                return;
            }
            else
            {
                panel2.Visible = false;
                TestTrueOrFalse = true;
                return;
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {

        }

        private void button4_Click_2(object sender, EventArgs e)
        {
            dataGridView3.DataSource = null;
            dataGridView3.Rows.Clear();
            dataGridView3.Columns.Clear();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            Form1 form1 = new Form1();

            form4.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
