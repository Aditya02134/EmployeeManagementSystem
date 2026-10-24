
using System;
using System.Drawing;
using System.Windows.Forms;
using EmployeeManagementSystem.Data;
using MySql.Data.MySqlClient;

namespace EmployeeManagementSystem.Forms
{
    public partial class EmployeeForm : Form
    {
        private TextBox txtEmployeeCode = null!;
        private TextBox txtFullName = null!;
        private TextBox txtEmail = null!;
        private TextBox txtPhone = null!;
        private TextBox txtDesignation = null!;
        private TextBox txtSalary = null!;
        private ComboBox cmbDepartment = null!;
        private DateTimePicker dtpJoiningDate = null!;
        private Button btnSave = null!;
        private Button btnClear = null!;

        private sealed class DepartmentItem
        {
            public int ID { get; }
            public string Name { get; }

            public DepartmentItem(int id, string name)
            {
                ID = id;
                Name = name;
            }

            public override string ToString() => Name;
        }

        public EmployeeForm()
        {
            InitializeComponent();
            Controls.Clear();
            CreateControls();
            LoadDepartments();
        }

        private void CreateControls()
        {
            Text = "Employee Registration";
            ClientSize = new Size(850, 760);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 10);
            AutoScaleMode = AutoScaleMode.None;

            // Heading: smaller font and adequate width.
            Label lblTitle = new Label
            {
                Text = "EMPLOYEE REGISTRATION",
                Location = new Point(25, 15),
                Size = new Size(800, 55),
                Font = new Font("Segoe UI", 21, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 55, 80),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                UseCompatibleTextRendering = true,
                Padding = new Padding(0)
            };
            Controls.Add(lblTitle);

            // Subtitle has its own separate row.
            Label lblSubtitle = new Label
            {
                Text = "Enter employee details below",
                Location = new Point(25, 73),
                Size = new Size(800, 38),
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                UseCompatibleTextRendering = true
            };
            Controls.Add(lblSubtitle);

            int labelX = 75;
            int inputX = 285;
            int labelWidth = 195;
            int inputWidth = 470;
            int startY = 130;
            int rowHeight = 58;

            AddFieldLabel("Employee Code", labelX, startY, labelWidth);
            txtEmployeeCode = AddTextBox(inputX, startY, inputWidth);

            AddFieldLabel("Full Name", labelX, startY + rowHeight, labelWidth);
            txtFullName = AddTextBox(inputX, startY + rowHeight, inputWidth);

            AddFieldLabel("Email Address", labelX, startY + rowHeight * 2, labelWidth);
            txtEmail = AddTextBox(inputX, startY + rowHeight * 2, inputWidth);

            AddFieldLabel("Phone Number", labelX, startY + rowHeight * 3, labelWidth);
            txtPhone = AddTextBox(inputX, startY + rowHeight * 3, inputWidth);

            AddFieldLabel("Department", labelX, startY + rowHeight * 4, labelWidth);

            cmbDepartment = new ComboBox
            {
                Location = new Point(inputX, startY + rowHeight * 4),
                Size = new Size(inputWidth, 38),
                Font = new Font("Segoe UI", 11),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            Controls.Add(cmbDepartment);

            AddFieldLabel("Designation", labelX, startY + rowHeight * 5, labelWidth);
            txtDesignation = AddTextBox(inputX, startY + rowHeight * 5, inputWidth);

            AddFieldLabel("Joining Date", labelX, startY + rowHeight * 6, labelWidth);

            dtpJoiningDate = new DateTimePicker
            {
                Location = new Point(inputX, startY + rowHeight * 6),
                Size = new Size(inputWidth, 38),
                Font = new Font("Segoe UI", 11),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd MMM yyyy"
            };
            Controls.Add(dtpJoiningDate);

            AddFieldLabel("Salary", labelX, startY + rowHeight * 7, labelWidth);
            txtSalary = AddTextBox(inputX, startY + rowHeight * 7, inputWidth);

            // Save and Clear buttons
            btnSave = CreateButton("Save Employee", 285, 610, 225);
            btnSave.Click += BtnSave_Click;

            btnClear = CreateButton("Clear Fields", 530, 610, 225);
            btnClear.BackColor = Color.FromArgb(100, 110, 125);
            btnClear.Click += (s, e) => ClearFields();

            Label lblFooter = new Label
            {
                Text = "All fields are required.",
                Location = new Point(75, 670),
                Size = new Size(680, 30),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(lblFooter);
        }

        private void AddFieldLabel(string text, int x, int y, int width)
        {
            Label label = new Label
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, 38),
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(45, 50, 60),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false
            };

            Controls.Add(label);
        }

        private TextBox AddTextBox(int x, int y, int width)
        {
            TextBox textBox = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, 38),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle
            };

            Controls.Add(textBox);
            return textBox;
        }

        private Button CreateButton(string text, int x, int y, int width)
        {
            Button button = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, 44),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(40, 100, 170),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            Controls.Add(button);
            return button;
        }

        private void LoadDepartments()
        {
            try
            {
                using (MySqlConnection connection =
                       DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    const string query =
                        "SELECT DepartmentID, DepartmentName " +
                        "FROM Departments ORDER BY DepartmentName";

                    using (MySqlCommand command =
                           new MySqlCommand(query, connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        cmbDepartment.Items.Clear();

                        while (reader.Read())
                        {
                            cmbDepartment.Items.Add(
                                new DepartmentItem(
                                    Convert.ToInt32(reader["DepartmentID"]),
                                    reader["DepartmentName"].ToString() ?? ""));
                        }
                    }
                }

                if (cmbDepartment.Items.Count > 0)
                    cmbDepartment.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load departments.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmployeeCode.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtDesignation.Text) ||
                string.IsNullOrWhiteSpace(txtSalary.Text))
            {
                MessageBox.Show(
                    "Please complete all fields.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (cmbDepartment.SelectedItem is not DepartmentItem department)
            {
                MessageBox.Show("Please select a department.");
                return;
            }

            if (!decimal.TryParse(txtSalary.Text.Trim(), out decimal salary) ||
                salary < 0)
            {
                MessageBox.Show("Please enter a valid non-negative salary.");
                txtSalary.Focus();
                return;
            }

            if (!System.Net.Mail.MailAddress.TryCreate(
                    txtEmail.Text.Trim(), out _))
            {
                MessageBox.Show("Please enter a valid email address.");
                txtEmail.Focus();
                return;
            }

            try
            {
                using (MySqlConnection connection =
                       DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    const string query = @"
                        INSERT INTO Employees
                        (EmployeeCode, FullName, Email, Phone,
                         DepartmentID, Designation, JoiningDate, Salary)
                        VALUES
                        (@code, @name, @email, @phone,
                         @department, @designation, @joiningDate, @salary)";

                    using (MySqlCommand command =
                           new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@code", txtEmployeeCode.Text.Trim());
                        command.Parameters.AddWithValue(
                            "@name", txtFullName.Text.Trim());
                        command.Parameters.AddWithValue(
                            "@email", txtEmail.Text.Trim());
                        command.Parameters.AddWithValue(
                            "@phone", txtPhone.Text.Trim());
                        command.Parameters.AddWithValue(
                            "@department", department.ID);
                        command.Parameters.AddWithValue(
                            "@designation", txtDesignation.Text.Trim());
                        command.Parameters.AddWithValue(
                            "@joiningDate", dtpJoiningDate.Value.Date);
                        command.Parameters.AddWithValue(
                            "@salary", salary);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Employee registered successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearFields();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    MessageBox.Show(
                        "Employee code or email already exists.",
                        "Duplicate Record",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Database error:\n" + ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not save employee:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            txtEmployeeCode.Clear();
            txtFullName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtDesignation.Clear();
            txtSalary.Clear();
            dtpJoiningDate.Value = DateTime.Today;

            if (cmbDepartment.Items.Count > 0)
                cmbDepartment.SelectedIndex = 0;

            txtEmployeeCode.Focus();
        }
    }
}
