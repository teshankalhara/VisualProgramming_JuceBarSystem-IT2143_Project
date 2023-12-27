using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualProgramming
{
    public partial class Customers : Form
    {
        Form1 form1 = null;
        Dashboard dashboard = null;
        Billing bill = null;
        Categories category = null;

        private string name;
        private string gender;
        private string phone;

        SqlConnection conn = new SqlConnection("Data Source=THIRANJAYA\\SQLEXPRESS;Initial Catalog=cafe;Integrated Security=True");

        public Customers()
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

        private void categories_Click(object sender, EventArgs e)
        {
            if (category == null || category.IsDisposed)
            {
                category = new Categories();
            }
            this.Hide();
            category.Show();
        }

        private void billing_Click(object sender, EventArgs e)
        {
            if (bill == null || bill.IsDisposed)
            {
                bill = new Billing();
            }
            this.Hide();
            bill.Show();
        }

        private void dashoboards_Click(object sender, EventArgs e)
        {
            if (dashboard == null || dashboard.IsDisposed)
            {
                dashboard = new Dashboard();
            }
            this.Hide();
            dashboard.Show();
        }

        private void addItemBtn_Click(object sender, EventArgs e)
        {
            if (validate())
            {
                inserNewCustomer();
            }
        }

        public void inserNewCustomer()
        {
            SqlCommand command = new SqlCommand("insert into table2 values(@name," +
                                                                           "@gender," +
                                                                           "@phone)", conn);
            command.CommandType = CommandType.Text;
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@gender", gender);
            command.Parameters.AddWithValue("@phone", phone);

            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
        }

        public bool validate()
        {
            Regex nameValidate = new Regex(@"^[a-zA-Z]");
            Regex phoneVAlidate = new Regex(@"^[0-9]{10}");
            errorProvider1.Clear();

            if (!nameValidate.IsMatch(customerName.Text) || string.IsNullOrEmpty(customerName.Text))
            {
                errorProvider1.SetError(customerName, "Alphabets Only");
                return false;
            }

            
            if (genderBox.SelectedItem == null)
            {
                errorProvider1.SetError(genderBox, "Select Gender");
                return false;
            }
            
            if (!phoneVAlidate.IsMatch(phoneBox.Text) || string.IsNullOrEmpty(phoneBox.Text))
            {
                errorProvider1.SetError(phoneBox, "Alphabets Only");
                return false;
            }
            return true;
        }

        public string getCustomerName()
        {
            return customerName.Text;
        }

        public string getGender()
        {
            return genderBox.SelectedItem.ToString();
        }
        
        public string getPhone()
        {
            return phoneBox.Text;
        }

        private void Customers_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'cafeDataSet1.Table2' table. You can move, or remove it, as needed.
            this.table2TableAdapter.Fill(this.cafeDataSet1.Table2);
            showCustomerTable();

        }

        public void showCustomerTable()
        {
            SqlCommand command = new SqlCommand("select * from Table2", conn);
            DataTable dt = new DataTable();
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            dt.Load(reader);
            conn.Close();
            dataGridView1.DataSource = dt;
        }

        private void customerName_TextChanged(object sender, EventArgs e)
        {
            name = this.customerName.Text;
        }

        private void genderBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(genderBox.SelectedItem.ToString() == "Male")
            {
                gender = "Male";
            }
            else
            {
                gender = "Femal";
            }
        }

        private void phoneBox_TextChanged(object sender, EventArgs e)
        {
            phone = this.phoneBox.Text;
        }
    }
}
