
using System;
using System.Drawing;
using System.Net.Mail;
using System.Windows.Forms;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using MySql.Data.MySqlClient;

namespace EmployeeManagementSystem.Forms
{
    public partial class EmployeeEditForm : Form
    {
        private readonly int employeeID;
        private readonly string originalEmployeeCode;
        private readonly string originalEmail;

        private TextBox txtCode = new TextBox();
        private TextBox txtName = new TextBox();
        private TextBox txtEmail = new TextBox();
        private TextBox txtPhone = new TextBox();
        private TextBox txtDesignation = new TextBox();
        private TextBox txtSalary = new TextBox();
        private ComboBox cmbDepartment = new ComboBox();
        private DateTimePicker dtpJoiningDate = new DateTimePicker();

        public EmployeeEditForm(
            int id, string code, string name, string email,
            string phone, int departmentID, string designation,
            DateTime joiningDate, decimal salary)
        {
            InitializeComponent();

            employeeID = id;
            originalEmployeeCode = code;
            originalEmail = email;

            Text = "Update Employee";
            Size = new Size(550, 570);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            int y = 20;

            AddField("Employee Code", txtCode, code, ref y);
            AddField("Full Name", txtName, name, ref y);
            AddField("Email", txtEmail, email, ref y);
            AddField("Phone", txtPhone, phone, ref y);

            Label lblDepartment = new Label
            {
                Text = "Department",
                Location = new Point(25, y + 5),
                AutoSize = true
            };
            Controls.Add(lblDepartment);

            cmbDepartment.Location = new Point(170, y);
            cmbDepartment.Size = new Size(300, 28);
            cmbDepartment.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(cmbDepartment);
            y += 45;

            AddField("Designation", txtDesignation, designation, ref y);

            Label lblDate = new Label
            {
                Text = "Joining Date",
                Location = new Point(25, y + 5),
                AutoSize = true
            };
            Controls.Add(lblDate);

            dtpJoiningDate.Location = new Point(170, y);
            dtpJoiningDate.Size = new Size(300, 28);
            dtpJoiningDate.Format = DateTimePickerFormat.Short;
            dtpJoiningDate.Value = joiningDate;
            Controls.Add(dtpJoiningDate);
            y += 45;

            AddField("Salary", txtSalary, salary.ToString("0.00"), ref y);

            Button btnSave = new Button
            {
                Text = "Save Changes",
                Location = new Point(170, y + 5),
                Size = new Size(140, 35)
            };
            btnSave.Click += BtnSave_Click;
            Controls.Add(btnSave);

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(320, y + 5),
                Size = new Size(100, 35),
                DialogResult = DialogResult.Cancel
            };
            Controls.Add(btnCancel);

            AcceptButton = btnSave;
            CancelButton = btnCancel;

            LoadDepartments(departmentID);
        }

        private void AddField(
            string labelText, TextBox textBox, string value, ref int y)
        {
            Label label = new Label
            {
                Text = labelText,
                Location = new Point(25, y + 5),
                AutoSize = true
            };
            Controls.Add(label);

            textBox.Location = new Point(170, y);
            textBox.Size = new Size(300, 28);
            textBox.Text = value;
            Controls.Add(textBox);

            y += 45;
        }

        private void LoadDepartments(int selectedDepartmentID)
        {
            try
            {
                using (MySqlConnection connection =
                       DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(
                        "SELECT DepartmentID, DepartmentName FROM Departments ORDER BY DepartmentName",
                        connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbDepartment.Items.Add(new Department
                            {
                                DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                                DepartmentName = reader["DepartmentName"].ToString()!
                            });
                        }
                    }
                }

                cmbDepartment.DisplayMember = "DepartmentName";
                cmbDepartment.ValueMember = "DepartmentID";

                for (int i = 0; i < cmbDepartment.Items.Count; i++)
                {
                    if (cmbDepartment.Items[i] is Department department &&
                        department.DepartmentID == selectedDepartmentID)
                    {
                        cmbDepartment.SelectedIndex = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load departments.\n" + ex.Message);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtDesignation.Text) ||
                string.IsNullOrWhiteSpace(txtSalary.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            try
            {
                var parsedEmail = new MailAddress(txtEmail.Text.Trim());
                if (parsedEmail.Address != txtEmail.Text.Trim())
                {
                    MessageBox.Show("Enter a valid email address.");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Enter a valid email address.");
                return;
            }

            if (!decimal.TryParse(txtSalary.Text, out decimal salary) ||
                salary < 0)
            {
                MessageBox.Show("Enter a valid non-negative salary.");
                return;
            }

            if (cmbDepartment.SelectedItem is not Department department)
            {
                MessageBox.Show("Select a department.");
                return;
            }

            try
            {
                using (MySqlConnection connection =
                       DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        UPDATE Employees
                        SET EmployeeCode = @code,
                            FullName = @name,
                            Email = @email,
                            Phone = @phone,
                            DepartmentID = @department,
                            Designation = @designation,
                            JoiningDate = @joiningDate,
                            Salary = @salary
                        WHERE EmployeeID = @id";

                    using (MySqlCommand command =
                           new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@code", txtCode.Text.Trim());
                        command.Parameters.AddWithValue("@name", txtName.Text.Trim());
                        command.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                        command.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                        command.Parameters.AddWithValue("@department", department.DepartmentID);
                        command.Parameters.AddWithValue("@designation", txtDesignation.Text.Trim());
                        command.Parameters.AddWithValue("@joiningDate", dtpJoiningDate.Value.Date);
                        command.Parameters.AddWithValue("@salary", salary);
                        command.Parameters.AddWithValue("@id", employeeID);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Employee updated successfully.");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                    MessageBox.Show(
                        "Employee code or email already belongs to another employee.");
                else
                    MessageBox.Show("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating employee: " + ex.Message);
            }
        }
    }
}
