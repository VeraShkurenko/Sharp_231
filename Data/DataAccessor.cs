using Sharp_231.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

namespace Sharp_231.Data
{
    internal class DataAccessor
    {
        private readonly MySqlConnection connection;

        public DataAccessor()
        {
            string connectionString = "Server=127.0.0.1;Port=3306;Database=demo;User Id=root;Password=veravira;";
            this.connection = new MySqlConnection(connectionString);
            try
            {
                this.connection.Open();
                EnsureDatabaseSetup();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Connection failed: " + ex.Message);
            }
        }

        private void EnsureDatabaseSetup()
        {
            string sqlProducts = @"CREATE TABLE IF NOT EXISTS Products(
                                        Id CHAR(36) PRIMARY KEY,
                                        Name VARCHAR(64) NOT NULL,
                                        Price DECIMAL(14,2) NOT NULL)";
            ExecuteNonQuery(sqlProducts);

            string sqlDepartments = @"CREATE TABLE IF NOT EXISTS Departments(
                                        Id CHAR(36) PRIMARY KEY,
                                        Name VARCHAR(64) NOT NULL)";
            ExecuteNonQuery(sqlDepartments);

            string sqlManagers = @"CREATE TABLE IF NOT EXISTS Managers(
                                        Id CHAR(36) PRIMARY KEY,
                                        DepartmentId CHAR(36) NOT NULL,
                                        Name VARCHAR(64) NOT NULL,
                                        WorksFrom DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP)";
            ExecuteNonQuery(sqlManagers);

            string sqlSales = @"CREATE TABLE IF NOT EXISTS Sales(
                                    Id CHAR(36) PRIMARY KEY,
                                    ManagerId CHAR(36) NOT NULL,
                                    ProductId CHAR(36) NOT NULL,
                                    Quantity INT NOT NULL DEFAULT 1,
                                    Moment DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP)";
            ExecuteNonQuery(sqlSales);
        }

        private void ExecuteNonQuery(string sql, Dictionary<string, object>? parameters = null)
        {
            using var cmd = new MySqlCommand(sql, connection);
            if (parameters != null)
                foreach (var p in parameters)
                    cmd.Parameters.AddWithValue(p.Key, p.Value);
            cmd.ExecuteNonQuery();
        }

        private T FromReader<T>(MySqlDataReader reader)
        {
            var t = typeof(T);
            var ctr = t.GetConstructor(Array.Empty<Type>());
            T res = (T)ctr!.Invoke(null);

            foreach (var prop in t.GetProperties())
            {
                try
                {
                    object val = reader[prop.Name];
                    if (val is decimal)
                        prop.SetValue(res, Convert.ToDouble(val));
                    else
                        prop.SetValue(res, val);
                }
                catch { }
            }
            return res;
        }

        private List<T> ExecuteList<T>(string sql, Dictionary<string, object>? parameters = null)
        {
            List<T> res = new();
            using var cmd = new MySqlCommand(sql, connection);
            if (parameters != null)
                foreach (var p in parameters)
                    cmd.Parameters.AddWithValue(p.Key, p.Value);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                res.Add(FromReader<T>(reader));

            return res;
        }

        private int ExecuteScalarInt(string sql, Dictionary<string, object>? parameters = null)
        {
            using var cmd = new MySqlCommand(sql, connection);
            if (parameters != null)
                foreach (var p in parameters)
                    cmd.Parameters.AddWithValue(p.Key, p.Value);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // -------------------- PRODUCTS --------------------
        public List<Product> GetProducts()
        {
            return ExecuteList<Product>("SELECT * FROM Products");
        }

        public void PrintNP() => GetProducts().OrderBy(p => p.Price).ToList().ForEach(p => Console.WriteLine($"{p.Name} -- {p.Price:F2}"));
        public void PrintDesc() => GetProducts().OrderByDescending(p => p.Price).ToList().ForEach(p => Console.WriteLine($"{p.Name} -- {p.Price:F2}"));
        public void PrintAlphabet() => GetProducts().OrderBy(p => p.Name).ToList().ForEach(p => Console.WriteLine($"{p.Name} -- {p.Price:F2}"));
        public void PrintLoser() => GetProducts().OrderBy(p => p.Price).Take(3).ToList().ForEach(p => Console.WriteLine($"{p.Name} -- {p.Price:F2}"));
        public void PrintTop() => GetProducts().OrderByDescending(p => p.Price).Take(3).ToList().ForEach(p => Console.WriteLine($"{p.Name} -- {p.Price:F2}"));

        // -------------------- SALES --------------------
        public int GetSalesCountByMonth(int month, int year)
        {
            string sql = @"SELECT COUNT(*) FROM Sales 
                           WHERE MONTH(Moment) = @month AND YEAR(Moment) = @year";
            return ExecuteScalarInt(sql, new() { ["@month"] = month, ["@year"] = year });
        }

        public (int currentYear, int previousYear) GetSalesInfoByMonth(int month)
        {
            int currentYear = DateTime.Now.Year;
            int previousYear = currentYear - 1;

            int current = GetSalesCountByMonth(month, currentYear);
            int previous = GetSalesCountByMonth(month, previousYear);

            return (current, previous);
        }
    }
}
