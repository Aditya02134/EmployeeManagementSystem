
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using EmployeeManagementSystem.Data;
using MySql.Data.MySqlClient;

namespace EmployeeManagementSystem.Forms
{
    public partial class DepartmentForm : Form
    {
        private TextBox txtDepartmentName = null!;
        private DataGridView dgvDepartments = null!;
        private Button btnAdd = null!;
        private Button btnUpdate = null!;
        private Button btnDelete = null!;
        private Button btnClear = null!;

        private int? selectedDepartmentID = null;

        public DepartmentForm()
        {
            InitializeComponent();
            Controls.Clear();
            CreateControls();
            LoadDepartments();
        }

        private void CreateControls()
        {
            Text = "Department Management";
            ClientSize = new Size(850, 650);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            // Main title
            Label lblTitle = new Label
            {
                Text = "DEPARTMENT MANAGEMENT",
                Location = new Point(25, 20),
                Size = new Size(800, 55),
                Font = new Font("Segoe UI", 19, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 55, 80),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };
            Controls.Add(lblTitle);

            // Subtitle
            Label lblSubtitle = new Label
            {
                Text = "Add, update and manage your departments",
                Location = new Point(25, 75),
                Size = new Size(800, 30),
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };
            Controls.Add(lblSubtitle);

            // Department name label
            Label lblName = new Label
            {
                Text = "Department Name:",
                Location = new Point(35, 130),
                Size = new Size(165, 32),
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleLeft
            };
            Controls.Add(lblName);

            // Department name input
            txtDepartmentName = new TextBox
            {
                Location = new Point(205, 130),
                Size = new Size(595, 32),
                Font = new Font("Segoe UI", 11)
            };
            Controls.Add(txtDepartmentName);

            // Action buttons
            btnAdd = CreateButton("Add", 35, 180, 175);
            btnAdd.Click += BtnAdd_Click;

            btnUpdate = CreateButton("Update", 225, 180, 175);
            btnUpdate.Click += BtnUpdate_Click;

            btnDelete = CreateButton("Delete", 415, 180, 175);
            btnDelete.Click += BtnDelete_Click;

            btnClear = CreateButton("Clear", 605, 180, 120);
            btnClear.Click += (s, e) => ClearSelection();

            // Department table
            dgvDepartments = new DataGridView
            {
                Name = "dgvDepartments",
                Location = new Point(35, 245),
                Size = new Size(765, 320),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 11),
                ColumnHeadersHeight = 40,
                EnableHeadersVisualStyles = false
            };

            dgvDepartments.RowTemplate.Height = 35;

            dgvDepartments.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(40, 100, 170);
            dgvDepartments.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;
            dgvDepartments.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);

            dgvDepartments.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(210, 230, 250);
            dgvDepartments.DefaultCellStyle.SelectionForeColor =
                Color.Black;

            dgvDepartments.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(245, 248, 252);

            dgvDepartments.CellClick += DgvDepartments_CellClick;
            Controls.Add(dgvDepartments);

            // Instruction at bottom
            Label lblHint = new Label
            {
                Text = "Select a department in the table to update or delete it.",
                Location = new Point(35, 580),
                Size = new Size(765, 30),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleLeft
            };
            Controls.Add(lblHint);
        }

        private Button CreateButton(string text, int x, int y, int width)
        {
            Button button = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, 42),
                Font = new Font("Segoe UI", 10),
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

                    string query = @"
                        SELECT DepartmentID, DepartmentName
                        FROM Departments
                        ORDER BY DepartmentName";

                    using (MySqlDataAdapter adapter =
                           new MySqlDataAdapter(query, connection))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dgvDepartments.DataSource = table;
                    }
                }

                if (dgvDepartments.Columns.Contains("DepartmentID"))
                {
                    dgvDepartments.Columns["DepartmentID"].Visible = false;
                }

                if (dgvDepartments.Columns.Contains("DepartmentName"))
                {
                    dgvDepartments.Columns["DepartmentName"].HeaderText =
                        "Department Name";
                    dgvDepartments.Columns["DepartmentName"].FillWeight = 100;
                }

                dgvDepartments.ClearSelection();
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

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            string name = txtDepartmentName.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a department name.");
                txtDepartmentName.Focus();
                return;
            }

            try
            {
                using (MySqlConnection connection =
                       DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query =
                        "INSERT INTO Departments (DepartmentName) VALUES (@name)";

                    using (MySqlCommand command =
                           new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Department added successfully.");
                ClearSelection();
                LoadDepartments();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                    MessageBox.Show("That department already exists.");
                else
                    MessageBox.Show("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void DgvDepartments_CellClick(
            object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvDepartments.Rows[e.RowIndex];

            selectedDepartmentID = Convert.ToInt32(
                row.Cells["DepartmentID"].Value);

            txtDepartmentName.Text =
                row.Cells["DepartmentName"].Value?.ToString() ?? "";
        }

        private void BtnUpdate_Click(object? sender, EventArgs e)
        {
            if (selectedDepartmentID == null)
            {
                MessageBox.Show("Please select a department from the table.");
                return;
            }

            string name = txtDepartmentName.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a department name.");
                return;
            }

            try
            {
                using (MySqlConnection connection =
                       DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        UPDATE Departments
                        SET DepartmentName = @name
                        WHERE DepartmentID = @id";

                    using (MySqlCommand command =
                           new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue(
                            "@id", selectedDepartmentID.Value);

                        int affected = command.ExecuteNonQuery();

                        if (affected > 0)
                            MessageBox.Show("Department updated successfully.");
                        else
                            MessageBox.Show("No changes were made.");
                    }
                }

                ClearSelection();
                LoadDepartments();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                    MessageBox.Show("That department name already exists.");
                else
                    MessageBox.Show("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (selectedDepartmentID == null)
            {
                MessageBox.Show("Please select a department from the table.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this department?",
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

                    string query =
                        "DELETE FROM Departments WHERE DepartmentID = @id";

                    using (MySqlCommand command =
                           new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@id", selectedDepartmentID.Value);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Department deleted successfully.");
                ClearSelection();
                LoadDepartments();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1451)
                {
                    MessageBox.Show(
                        "This department is assigned to employees. " +
                        "Reassign those employees before deleting it.");
                }
                else
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ClearSelection()
        {
            selectedDepartmentID = null;
            txtDepartmentName.Clear();
            dgvDepartments.ClearSelection();
            txtDepartmentName.Focus();
        }
    }
}
