
using MySql.Data.MySqlClient;

namespace EmployeeManagementSystem.Data
{
    public static class DatabaseConnection
    {
        private static readonly string connectionString =
            "Server=localhost;" +
            "Port=3306;" +
            "Database=EmployeeManagementDB;" +
            "Uid=root;" +
            "Pwd=;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
