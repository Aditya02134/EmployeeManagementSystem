
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace EmployeeManagementSystem.Forms
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
            Controls.Clear();
            BuildMenu();
        }

        private void BuildMenu()
        {
            Text = "Employee Management System";
            ClientSize = new Size(800, 580);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.White;

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 7,
                Padding = new Padding(30, 25, 30, 25),
                BackColor = Color.White
            };

            layout.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100));

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));

            Label title = new Label
            {
                Text = "EMPLOYEE MANAGEMENT SYSTEM",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 19, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 55, 80),
                AutoSize = false
            };
            layout.Controls.Add(title, 0, 0);

            Label subtitle = new Label
            {
                Text = "Manage employee records efficiently",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Gray,
                AutoSize = false
            };
            layout.Controls.Add(subtitle, 0, 1);

            layout.Controls.Add(new Panel(), 0, 2);

            RoundedButton register =
                CreateButton("Register New Employee");

            register.Click += (s, e) =>
            {
                using (EmployeeForm form = new EmployeeForm())
                {
                    form.ShowDialog(this);
                }
            };
            layout.Controls.Add(register, 0, 3);

            RoundedButton view =
                CreateButton("View / Search Employees");

            view.Click += (s, e) =>
            {
                using (EmployeeListForm form = new EmployeeListForm())
                {
                    form.ShowDialog(this);
                }
            };
            layout.Controls.Add(view, 0, 4);

            RoundedButton departments =
                CreateButton("Manage Departments");

            departments.Click += (s, e) =>
            {
                using (DepartmentForm form = new DepartmentForm())
                {
                    form.ShowDialog(this);
                }
            };
            layout.Controls.Add(departments, 0, 5);

            RoundedButton exit =
                CreateButton("Exit Application");

            exit.BackColor = Color.FromArgb(90, 100, 115);
            exit.HoverColor = Color.FromArgb(65, 75, 90);
            exit.Click += (s, e) => Close();
            layout.Controls.Add(exit, 0, 6);

            Controls.Add(layout);
        }

        private RoundedButton CreateButton(string text)
        {
            return new RoundedButton
            {
                Text = text,
                Size = new Size(400, 46),
                Anchor = AnchorStyles.None,
                Margin = new Padding(5),
                Font = new Font("Segoe UI", 12),
                BackColor = Color.FromArgb(40, 100, 170),
                ForeColor = Color.White,
                HoverColor = Color.FromArgb(30, 80, 145),
                CornerRadius = 12,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat
            };
        }
    }

    public class RoundedButton : Button
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public int CornerRadius { get; set; } = 12;

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public Color HoverColor { get; set; } =
            Color.FromArgb(30, 80, 145);

        private bool isHovered;

        public RoundedButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint, true);

            MouseEnter += (s, e) =>
            {
                isHovered = true;
                Invalidate();
            };

            MouseLeave += (s, e) =>
            {
                isHovered = false;
                Invalidate();
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Parent?.BackColor ?? Color.White);

            Rectangle bounds =
                new Rectangle(0, 0, Width - 1, Height - 1);

            int radius = Math.Min(
                CornerRadius,
                Math.Min(Width, Height) / 2);

            using (GraphicsPath path =
                   CreateRoundedPath(bounds, radius))
            using (SolidBrush brush = new SolidBrush(
                   isHovered ? HoverColor : BackColor))
            {
                e.Graphics.FillPath(brush, path);

                TextRenderer.DrawText(
                    e.Graphics,
                    Text,
                    Font,
                    bounds,
                    ForeColor,
                    TextFormatFlags.HorizontalCenter |
                    TextFormatFlags.VerticalCenter |
                    TextFormatFlags.SingleLine);
            }
        }

        private GraphicsPath CreateRoundedPath(
            Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(
                rectangle.X, rectangle.Y,
                diameter, diameter, 180, 90);

            path.AddArc(
                rectangle.Right - diameter, rectangle.Y,
                diameter, diameter, 270, 90);

            path.AddArc(
                rectangle.Right - diameter,
                rectangle.Bottom - diameter,
                diameter, diameter, 0, 90);

            path.AddArc(
                rectangle.X,
                rectangle.Bottom - diameter,
                diameter, diameter, 90, 90);

            path.CloseFigure();
            return path;
        }
    }
}
