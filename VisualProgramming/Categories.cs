using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace VisualProgramming
{
    public partial class Categories : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=THIRANJAYA\\SQLEXPRESS;Initial Catalog=cafe;Integrated Security=True");

        Form1 form1 = null;
        Dashboard dashboard = null;
        Billing bill = null;
        Customers customer = null;

        private string foodName;
        private string itemName;
        private string itemPrice;
        private string itemCategory;

        public Categories()
        {
            InitializeComponent();
        }

        private void closeBtnItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            if (form1 == null || form1.IsDisposed)
            {
                form1 = new Form1();
            }
            this.Hide();
            form1.Show();
        }

        private void closeBtnItem_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void customers_Click(object sender, EventArgs e)
        {
            if (customer == null || customer.IsDisposed)
            {
                customer = new Customers();
            }
            this.Hide();
            customer.Show();
        }

        private void dashoboards_Click(object sender, EventArgs e)
        {
            if(dashboard == null || dashboard.IsDisposed)
            {
                dashboard = new Dashboard();
            }
            this.Hide();
            dashboard.Show();
        }

        private void billing_Click(object sender, EventArgs e)
        {
           if(bill == null || bill.IsDisposed)
            {
                bill = new Billing();
            }
            this.Hide();
            bill.Show();
        }

        private void addItemBtn_Click(object sender, EventArgs e)
        {
            if (validate())
            {
                inserNewItems();
                MessageBox.Show("Added OK!");
            }
        }

        public void inserNewItems()
        {
            SqlCommand command = new SqlCommand("insert into table1 values(@name," +
                                                                           "@category," +
                                                                           "@price)", conn);
            command.CommandType = CommandType.Text;
            command.Parameters.AddWithValue("@name", itemName);
            command.Parameters.AddWithValue("@category", itemCategory);
            command.Parameters.AddWithValue("@price", itemPrice);

            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
        }

        public string getItemName()
        {
            return itemNameBox.Text;
        }

        public string getItemPrice()
        {
            return itemPriceBox.Text;
        }

        public string getItemCategoryType()
        {
            return itemTypeBox.SelectedItem.ToString();
        }

        public bool validate()
        {
            Regex priceValidate = new Regex(@"^[0-9]");

            errorProvider.Clear();

            if (string.IsNullOrEmpty(itemNameBox.Text))
            {
                errorProvider.SetError(itemNameBox, "Only Allow Numberic Values!");
                return false;
            }

            if (!priceValidate.IsMatch(itemPriceBox.Text) || string.IsNullOrEmpty(itemPriceBox.Text))
            {
                errorProvider.SetError(itemPriceBox, "Only Allow Numberic Values!");
                return false;
            }

            if(itemTypeBox.SelectedItem == null)
            {
                errorProvider.SetError(itemTypeBox, "Only Allow Numberic Values!");
                return false;
            }

            return true;
        }

        private void Categories_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'cafeDataSet.table1' table. You can move, or remove it, as needed.
            this.table1TableAdapter.Fill(this.cafeDataSet.table1);
            showDrinkTable();
            showCakeTable();
        }

        public void showDrinkTable()
        {
            SqlCommand command = new SqlCommand("select * from table1 where category = 'Drink'", conn);
            DataTable dt = new DataTable();
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            dt.Load(reader);
            conn.Close();
            dataDrinks.DataSource = dt;
        }

        public void showCakeTable()
        {
            SqlCommand command = new SqlCommand("select * from table1 where category = 'Cake'", conn);
            DataTable dt = new DataTable();
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            dt.Load(reader);
            conn.Close();
            dataCake.DataSource = dt;
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            string query = "update table1 set name = '" + itemName + "', category = '" + itemCategory + "', price = '" + itemPrice + "' where name = '" + foodName + "'";
            sqlQueryExecute(query);
        }

        public void sqlQueryExecute(string query)
        {
            SqlCommand command = new SqlCommand(query, conn);
            command.CommandType = CommandType.Text;
            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
        }

        private void dataDrinks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataDrinks.Rows[e.RowIndex];
                foodName = Convert.ToString(selectedRow.Cells[0].Value);
            }
        }

        private void dataCake_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataCake.Rows[e.RowIndex];
                foodName = Convert.ToString(selectedRow.Cells[0].Value);
            }
        }

        private void itemNameBox_TextChanged(object sender, EventArgs e)
        {
            itemName = this.itemNameBox.Text;
        }

        private void itemPriceBox_TextChanged(object sender, EventArgs e)
        {
            itemPrice = this.itemPriceBox.Text;
        }

        private void itemTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(itemTypeBox.SelectedItem.ToString() == "Cake")
            {
                itemCategory = "Cake";
            }
            else if(itemTypeBox.SelectedItem.ToString() == "Drink")
            {
                itemCategory = "Drink";
            }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            string query = "delete from table1 where name = '" + foodName + "'";
            sqlQueryExecute(query);
        }
    }
}
