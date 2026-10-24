
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using EmployeeManagementSystem.Data;
using MySql.Data.MySqlClient;

namespace EmployeeManagementSystem.Forms
{
    public partial class EmployeeListForm : Form
    {
        private TextBox txtSearch = null!;
        private DataGridView dgvEmployees = null!;
        private Button btnRefresh = null!;
        private Button btnUpdate = null!;
        private Button btnDelete = null!;
        private Button btnClose = null!;

        private int? selectedEmployeeID;

        public EmployeeListForm()
        {
            InitializeComponent();
            Controls.Clear();
            CreateControls();
            LoadEmployees();
        }

        private void CreateControls()
        {
            Text = "Employee Management System - Employee List";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1000, 650);
            BackColor = Color.White;
            Font = new Font("Segoe UI", 10);
            AutoScaleMode = AutoScaleMode.Dpi;

            TableLayoutPanel main = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(24),
                BackColor = Color.White
            };

            main.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100));

            main.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 110));
            main.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 65));
            main.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));
            main.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 55));

            // Heading
            Panel headingPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            Label lblTitle = new Label
            {
                Text = "Employee Records",
                Location = new Point(0, 0),
                Size = new Size(1000, 60),
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 55, 80),
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false
            };

            Label lblSubtitle = new Label
            {
                Text = "View, search, update and delete employee records",
                Location = new Point(0, 63),
                Size = new Size(1000, 32),
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false
            };

            headingPanel.Controls.Add(lblTitle);
            headingPanel.Controls.Add(lblSubtitle);
            main.Controls.Add(headingPanel, 0, 0);

            // Search and buttons
            TableLayoutPanel actions = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 5,
                RowCount = 1,
                BackColor = Color.White,
                Padding = new Padding(0, 8, 0, 8),
                Margin = new Padding(0)
            };

            actions.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 150));
            actions.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100));
            actions.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 115));
            actions.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 125));
            actions.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 125));

            actions.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100));

            Label lblSearch = new Label
            {
                Text = "Search employees:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0)
            };
            actions.Controls.Add(lblSearch, 0, 0);

            txtSearch = new TextBox
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(5, 4, 12, 4),
                Font = new Font("Segoe UI", 11)
            };

            txtSearch.TextChanged += (s, e) => LoadEmployees();
            actions.Controls.Add(txtSearch, 1, 0);

            btnRefresh = CreateButton("Refresh");
            btnRefresh.Dock = DockStyle.Fill;
            btnRefresh.Margin = new Padding(4);
            btnRefresh.Click += (s, e) => LoadEmployees();
            actions.Controls.Add(btnRefresh, 2, 0);

            btnUpdate = CreateButton("Update");
            btnUpdate.Dock = DockStyle.Fill;
            btnUpdate.Margin = new Padding(4);
            btnUpdate.Click += BtnUpdate_Click;
            actions.Controls.Add(btnUpdate, 3, 0);

            btnDelete = CreateButton("Delete");
            btnDelete.Dock = DockStyle.Fill;
            btnDelete.Margin = new Padding(4);
            btnDelete.Click += BtnDelete_Click;
            actions.Controls.Add(btnDelete, 4, 0);

            main.Controls.Add(actions, 0, 1);

            // Employee grid
            dgvEmployees = new DataGridView
            {
                Name = "dgvEmployees",
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 5, 0, 5),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,
                ScrollBars = ScrollBars.Both,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10),
                ColumnHeadersHeight = 45,
                EnableHeadersVisualStyles = false
            };

            dgvEmployees.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(40, 100, 170);

            dgvEmployees.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            dgvEmployees.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);

            dgvEmployees.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(210, 230, 250);

            dgvEmployees.DefaultCellStyle.SelectionForeColor =
                Color.Black;

            dgvEmployees.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(245, 248, 252);

            dgvEmployees.CellClick += DgvEmployees_CellClick;
            dgvEmployees.SelectionChanged +=
                DgvEmployees_SelectionChanged;

            main.Controls.Add(dgvEmployees, 0, 2);

            // Footer
            Panel footer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(0)
            };

            btnClose = CreateButton("Close");
            btnClose.Size = new Size(120, 38);
            btnClose.Anchor =
                AnchorStyles.Right | AnchorStyles.Bottom;
            btnClose.Click += (s, e) => Close();

            footer.Controls.Add(btnClose);

            footer.Resize += (s, e) =>
            {
                btnClose.Location = new Point(
                    footer.ClientSize.Width - btnClose.Width,
                    Math.Max(0,
                        (footer.ClientSize.Height - btnClose.Height) / 2));
            };

            main.Controls.Add(footer, 0, 3);
            Controls.Add(main);
        }

        private Button CreateButton(string text)
        {
            Button button = new Button
            {
                Text = text,
                Size = new Size(110, 38),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(40, 100, 170),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private void LoadEmployees()
        {
            try
            {
                using (MySqlConnection connection =
                       DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    const string query = @"
                        SELECT
                            e.EmployeeID,
                            e.EmployeeCode,
                            e.FullName,
                            e.Email,
                            e.Phone,
                            d.DepartmentName,
                            e.Designation,
                            e.JoiningDate,
                            e.Salary
                        FROM Employees e
                        INNER JOIN Departments d
                            ON e.DepartmentID = d.DepartmentID
                        WHERE
                            e.EmployeeCode LIKE @search
                            OR e.FullName LIKE @search
                            OR e.Email LIKE @search
                            OR e.Phone LIKE @search
                            OR d.DepartmentName LIKE @search
                            OR e.Designation LIKE @search
                        ORDER BY e.EmployeeID DESC";

                    using (MySqlCommand command =
                           new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@search", "%" + txtSearch.Text.Trim() + "%");

                        using (MySqlDataAdapter adapter =
                               new MySqlDataAdapter(command))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);
                            dgvEmployees.DataSource = table;
                        }
                    }
                }

                ConfigureColumns();
                selectedEmployeeID = null;
                dgvEmployees.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load employees.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigureColumns()
        {
            if (dgvEmployees.Columns.Contains("EmployeeID"))
                dgvEmployees.Columns["EmployeeID"].Visible = false;

            SetColumn("EmployeeCode", "Employee Code", 120);
            SetColumn("FullName", "Full Name", 150);
            SetColumn("Email", "Email Address", 200);
            SetColumn("Phone", "Phone", 120);
            SetColumn("DepartmentName", "Department", 130);
            SetColumn("Designation", "Designation", 150);
            SetColumn("JoiningDate", "Joining Date", 130);
            SetColumn("Salary", "Salary", 110);

            if (dgvEmployees.Columns.Contains("JoiningDate"))
                dgvEmployees.Columns["JoiningDate"]
                    .DefaultCellStyle.Format = "dd-MMM-yyyy";

            if (dgvEmployees.Columns.Contains("Salary"))
                dgvEmployees.Columns["Salary"]
                    .DefaultCellStyle.Format = "N2";
        }

        private void SetColumn(
            string name, string heading, int fillWeight)
        {
            if (dgvEmployees.Columns.Contains(name))
            {
                DataGridViewColumn column =
                    dgvEmployees.Columns[name];

                column.HeaderText = heading;
                column.FillWeight = fillWeight;
                column.MinimumWidth = 85;
                column.AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void DgvEmployees_CellClick(
            object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                SelectEmployeeFromRow(dgvEmployees.Rows[e.RowIndex]);
        }

        private void DgvEmployees_SelectionChanged(
            object? sender, EventArgs e)
        {
            if (dgvEmployees.CurrentRow != null &&
                !dgvEmployees.CurrentRow.IsNewRow)
            {
                SelectEmployeeFromRow(dgvEmployees.CurrentRow);
            }
        }

        private void SelectEmployeeFromRow(DataGridViewRow row)
        {
            if (row.Cells["EmployeeID"].Value != null &&
                int.TryParse(
                    row.Cells["EmployeeID"].Value.ToString(),
                    out int id))
            {
                selectedEmployeeID = id;
            }
        }

        private void BtnUpdate_Click(object? sender, EventArgs e)
        {
            if (selectedEmployeeID == null)
            {
                MessageBox.Show(
                    "Please select an employee from the table first.",
                    "No Employee Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            using (EmployeeEditDialog editForm =
                   new EmployeeEditDialog(selectedEmployeeID.Value))
            {
                if (editForm.ShowDialog(this) == DialogResult.OK)
                    LoadEmployees();
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (selectedEmployeeID == null)
            {
                MessageBox.Show(
                    "Please select an employee to delete.",
                    "No Employee Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this employee?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (MySqlConnection connection =
                       DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    const string query =
                        "DELETE FROM Employees WHERE EmployeeID = @id";

                    using (MySqlCommand command =
                           new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@id", selectedEmployeeID.Value);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Employee deleted successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not delete employee.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }

    // Separate dialog used to edit an existing employee.
    internal sealed class EmployeeEditDialog : Form
    {
        private readonly int employeeID;

        private TextBox txtCode = null!;
        private TextBox txtName = null!;
        private TextBox txtEmail = null!;
        private TextBox txtPhone = null!;
        private TextBox txtDesignation = null!;
        private TextBox txtSalary = null!;
        private ComboBox cmbDepartment = null!;
        private DateTimePicker dtpJoiningDate = null!;

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

        public EmployeeEditDialog(int id)
        {
            employeeID = id;
            CreateControls();
            LoadDepartmentsAndEmployee();
        }

        private void CreateControls()
        {
            Text = "Update Employee";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(650, 610);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 10);

            Label title = new Label
            {
                Text = "UPDATE EMPLOYEE",
                Location = new Point(20, 15),
                Size = new Size(610, 45),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 55, 80),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(title);

            int y = 80;
            AddLabel("Employee Code", y);
            txtCode = AddTextBox(y);
            y += 55;

            AddLabel("Full Name", y);
            txtName = AddTextBox(y);
            y += 55;

            AddLabel("Email", y);
            txtEmail = AddTextBox(y);
            y += 55;

            AddLabel("Phone", y);
            txtPhone = AddTextBox(y);
            y += 55;

            AddLabel("Department", y);
            cmbDepartment = new ComboBox
            {
                Location = new Point(210, y),
                Size = new Size(390, 34),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            Controls.Add(cmbDepartment);
            y += 55;

            AddLabel("Designation", y);
            txtDesignation = AddTextBox(y);
            y += 55;

            AddLabel("Joining Date", y);
            dtpJoiningDate = new DateTimePicker
            {
                Location = new Point(210, y),
                Size = new Size(390, 34),
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd MMM yyyy"
            };
            Controls.Add(dtpJoiningDate);
            y += 55;

            AddLabel("Salary", y);
            txtSalary = AddTextBox(y);

            Button save = CreateButton("Save Changes", 210, 535, 185);
            save.Click += SaveChanges_Click;

            Button cancel = CreateButton("Cancel", 415, 535, 185);
            cancel.BackColor = Color.FromArgb(100, 110, 125);
            cancel.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };
        }

        private void AddLabel(string text, int y)
        {
            Controls.Add(new Label
            {
                Text = text,
                Location = new Point(35, y),
                Size = new Size(165, 34),
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleLeft
            });
        }

        private TextBox AddTextBox(int y)
        {
            TextBox textBox = new TextBox
            {
                Location = new Point(210, y),
                Size = new Size(390, 34),
                Font = new Font("Segoe UI", 10)
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
                Size = new Size(width, 42),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(40, 100, 170),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            Controls.Add(button);
            return button;
        }

        private void LoadDepartmentsAndEmployee()
        {
            try
            {
                using (MySqlConnection connection =
                       DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    const string departmentsQuery =
                        "SELECT DepartmentID, DepartmentName " +
                        "FROM Departments ORDER BY DepartmentName";

                    using (MySqlCommand command =
                           new MySqlCommand(departmentsQuery, connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbDepartment.Items.Add(
                                new DepartmentItem(
                                    Convert.ToInt32(reader["DepartmentID"]),
                                    reader["DepartmentName"].ToString() ?? ""));
                        }
                    }

                    const string employeeQuery = @"
                        SELECT EmployeeCode, FullName, Email, Phone,
                               DepartmentID, Designation, JoiningDate, Salary
                        FROM Employees
                        WHERE EmployeeID = @id";

                    using (MySqlCommand command =
                           new MySqlCommand(employeeQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", employeeID);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("Employee record not found.");
                                DialogResult = DialogResult.Cancel;
                                Close();
                                return;
                            }

                            txtCode.Text = reader["EmployeeCode"].ToString();
                            txtName.Text = reader["FullName"].ToString();
                            txtEmail.Text = reader["Email"].ToString();
                            txtPhone.Text = reader["Phone"].ToString();
                            txtDesignation.Text = reader["Designation"].ToString();
                            txtSalary.Text = Convert.ToDecimal(
                                reader["Salary"]).ToString("0.00");

                            dtpJoiningDate.Value =
                                Convert.ToDateTime(reader["JoiningDate"]);

                            int departmentID =
                                Convert.ToInt32(reader["DepartmentID"]);

                            for (int i = 0; i < cmbDepartment.Items.Count; i++)
                            {
                                if (cmbDepartment.Items[i] is DepartmentItem item &&
                                    item.ID == departmentID)
                                {
                                    cmbDepartment.SelectedIndex = i;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load employee details.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void SaveChanges_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtDesignation.Text) ||
                string.IsNullOrWhiteSpace(txtSalary.Text))
            {
                MessageBox.Show("Please complete all fields.");
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
                MessageBox.Show("Enter a valid non-negative salary.");
                txtSalary.Focus();
                return;
            }

            if (!System.Net.Mail.MailAddress.TryCreate(
                    txtEmail.Text.Trim(), out _))
            {
                MessageBox.Show("Enter a valid email address.");
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
                        UPDATE Employees SET
                            EmployeeCode = @code,
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
                        command.Parameters.AddWithValue("@department", department.ID);
                        command.Parameters.AddWithValue("@designation", txtDesignation.Text.Trim());
                        command.Parameters.AddWithValue("@joiningDate", dtpJoiningDate.Value.Date);
                        command.Parameters.AddWithValue("@salary", salary);
                        command.Parameters.AddWithValue("@id", employeeID);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Employee updated successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    MessageBox.Show(
                        "That employee code or email is already used by another employee.",
                        "Duplicate Record",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Database error:\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not update employee:\n" + ex.Message);
            }
        }
    }
}
