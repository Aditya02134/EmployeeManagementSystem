
namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }

        public string EmployeeCode { get; set; } = "";

        public string FullName { get; set; } = "";

        public string Email { get; set; } = "";

        public string Phone { get; set; } = "";

        public int DepartmentID { get; set; }

        public string Designation { get; set; } = "";

        public DateTime JoiningDate { get; set; }

        public decimal Salary { get; set; }
    }
}
