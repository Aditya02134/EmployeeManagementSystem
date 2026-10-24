
using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace EmployeeManagementSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // The form loads here.
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString =
                "Server=localhost;" +
                "Port=3306;" +
                "Database=EmployeeManagementDB;" +
                "Uid=root;" +
                "Pwd=;";

            try
            {
                using (MySqlConnection connection =
                       new MySqlConnection(connectionString))
                {
                    connection.Open();

                    MessageBox.Show(
                        "Successfully connected to EmployeeManagementDB!",
                        "Connection Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Database connection failed.\n\n" + ex.Message,
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
