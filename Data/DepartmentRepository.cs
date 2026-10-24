
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Data
{
    public class DepartmentRepository
    {
        // Get all departments from the database
        public List<Department> GetAllDepartments()
        {
            List<Department> departments = new List<Department>();

            string query = @"
                SELECT DepartmentID, DepartmentName
                FROM Departments
                ORDER BY DepartmentName";

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
                        Department department = new Department
                        {
                            DepartmentID = Convert.ToInt32(
                                reader["DepartmentID"]),
                            DepartmentName = reader["DepartmentName"].ToString()!
                        };

                        departments.Add(department);
                    }
                }
            }

            return departments;
        }

        // Add a new department
        public bool AddDepartment(string departmentName)
        {
            string query = @"
                INSERT INTO Departments (DepartmentName)
                VALUES (@DepartmentName)";

            using (MySqlConnection connection =
                   DatabaseConnection.GetConnection())
            using (MySqlCommand command =
                   new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@DepartmentName", departmentName.Trim());

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        // Update an existing department
        public bool UpdateDepartment(int departmentID, string departmentName)
        {
            string query = @"
                UPDATE Departments
                SET DepartmentName = @DepartmentName
                WHERE DepartmentID = @DepartmentID";

            using (MySqlConnection connection =
                   DatabaseConnection.GetConnection())
            using (MySqlCommand command =
                   new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@DepartmentName", departmentName.Trim());

                command.Parameters.AddWithValue(
                    "@DepartmentID", departmentID);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        // Delete a department
        public bool DeleteDepartment(int departmentID)
        {
            string query = @"
                DELETE FROM Departments
                WHERE DepartmentID = @DepartmentID";

            using (MySqlConnection connection =
                   DatabaseConnection.GetConnection())
            using (MySqlCommand command =
                   new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@DepartmentID", departmentID);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}
