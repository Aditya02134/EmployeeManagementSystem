
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Data
{
    public class EmployeeRepository
    {
        // Add a new employee
        public bool AddEmployee(Employee employee)
        {
            string query = @"
                INSERT INTO Employees
                (EmployeeCode, FullName, Email, Phone,
                 DepartmentID, Designation, JoiningDate, Salary)
                VALUES
                (@EmployeeCode, @FullName, @Email, @Phone,
                 @DepartmentID, @Designation, @JoiningDate, @Salary)";

            using (MySqlConnection connection =
                   DatabaseConnection.GetConnection())
            using (MySqlCommand command =
                   new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@EmployeeCode", employee.EmployeeCode);
                command.Parameters.AddWithValue(
                    "@FullName", employee.FullName);
                command.Parameters.AddWithValue(
                    "@Email", employee.Email);
                command.Parameters.AddWithValue(
                    "@Phone", employee.Phone);
                command.Parameters.AddWithValue(
                    "@DepartmentID", employee.DepartmentID);
                command.Parameters.AddWithValue(
                    "@Designation", employee.Designation);
                command.Parameters.AddWithValue(
                    "@JoiningDate", employee.JoiningDate.Date);
                command.Parameters.AddWithValue(
                    "@Salary", employee.Salary);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        // Get all employees
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            string query = @"
                SELECT EmployeeID, EmployeeCode, FullName, Email,
                       Phone, DepartmentID, Designation, JoiningDate, Salary
                FROM Employees
                ORDER BY EmployeeID DESC";

            using (MySqlConnection connection =
                   DatabaseConnection.GetConnection())
            using (MySqlCommand command =
                   new MySqlCommand(query, connection))
            {
                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Employee employee = new Employee
                        {
                            EmployeeID = Convert.ToInt32(
                                reader["EmployeeID"]),
                            EmployeeCode = reader["EmployeeCode"].ToString()!,
                            FullName = reader["FullName"].ToString()!,
                            Email = reader["Email"].ToString()!,
                            Phone = reader["Phone"].ToString()!,
                            DepartmentID = Convert.ToInt32(
                                reader["DepartmentID"]),
                            Designation = reader["Designation"].ToString()!,
                            JoiningDate = Convert.ToDateTime(
                                reader["JoiningDate"]),
                            Salary = Convert.ToDecimal(reader["Salary"])
                        };

                        employees.Add(employee);
                    }
                }
            }

            return employees;
        }

        // Update an employee
        public bool UpdateEmployee(Employee employee)
        {
            string query = @"
                UPDATE Employees
                SET EmployeeCode = @EmployeeCode,
                    FullName = @FullName,
                    Email = @Email,
                    Phone = @Phone,
                    DepartmentID = @DepartmentID,
                    Designation = @Designation,
                    JoiningDate = @JoiningDate,
                    Salary = @Salary
                WHERE EmployeeID = @EmployeeID";

            using (MySqlConnection connection =
                   DatabaseConnection.GetConnection())
            using (MySqlCommand command =
                   new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@EmployeeCode", employee.EmployeeCode);
                command.Parameters.AddWithValue(
                    "@FullName", employee.FullName);
                command.Parameters.AddWithValue(
                    "@Email", employee.Email);
                command.Parameters.AddWithValue(
                    "@Phone", employee.Phone);
                command.Parameters.AddWithValue(
                    "@DepartmentID", employee.DepartmentID);
                command.Parameters.AddWithValue(
                    "@Designation", employee.Designation);
                command.Parameters.AddWithValue(
                    "@JoiningDate", employee.JoiningDate.Date);
                command.Parameters.AddWithValue(
                    "@Salary", employee.Salary);
                command.Parameters.AddWithValue(
                    "@EmployeeID", employee.EmployeeID);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        // Delete an employee
        public bool DeleteEmployee(int employeeID)
        {
            string query =
                "DELETE FROM Employees WHERE EmployeeID = @EmployeeID";

            using (MySqlConnection connection =
                   DatabaseConnection.GetConnection())
            using (MySqlCommand command =
                   new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@EmployeeID", employeeID);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        // Search employees by name, code, email or designation
        public List<Employee> SearchEmployees(string searchText)
        {
            List<Employee> employees = new List<Employee>();

            string query = @"
                SELECT EmployeeID, EmployeeCode, FullName, Email,
                       Phone, DepartmentID, Designation, JoiningDate, Salary
                FROM Employees
                WHERE EmployeeCode LIKE @Search
                   OR FullName LIKE @Search
                   OR Email LIKE @Search
                   OR Designation LIKE @Search
                ORDER BY FullName";

            using (MySqlConnection connection =
                   DatabaseConnection.GetConnection())
            using (MySqlCommand command =
                   new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@Search", "%" + searchText + "%");

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Employee employee = new Employee
                        {
                            EmployeeID = Convert.ToInt32(
                                reader["EmployeeID"]),
                            EmployeeCode = reader["EmployeeCode"].ToString()!,
                            FullName = reader["FullName"].ToString()!,
                            Email = reader["Email"].ToString()!,
                            Phone = reader["Phone"].ToString()!,
                            DepartmentID = Convert.ToInt32(
                                reader["DepartmentID"]),
                            Designation = reader["Designation"].ToString()!,
                            JoiningDate = Convert.ToDateTime(
                                reader["JoiningDate"]),
                            Salary = Convert.ToDecimal(reader["Salary"])
                        };

                        employees.Add(employee);
                    }
                }
            }

            return employees;
        }
    }
}
