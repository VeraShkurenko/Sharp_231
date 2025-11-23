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
                EnsureDatabaseSetup(); // <-- Додаємо створення таблиці і даних відразу
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Connection failed: " + ex.Message);
            }
        }

        private void EnsureDatabaseSetup()
        {
            // Створюємо таблицю, якщо її нема
            string sqlCreateTable = @"CREATE TABLE IF NOT EXISTS Products(
                                        Id CHAR(36) PRIMARY KEY,
                                        Name VARCHAR(64) NOT NULL,
                                        Price DECIMAL(14,2) NOT NULL)";
            using var cmd = new MySqlCommand(sqlCreateTable, connection);
            cmd.ExecuteNonQuery();

            // Перевіряємо, чи є записи. Якщо ні — додаємо
            string sqlCheck = "SELECT COUNT(*) FROM Products";
            cmd.CommandText = sqlCheck;
            long count = (long)cmd.ExecuteScalar();
            if (count == 0)
            {
                string sqlSeed = @"INSERT INTO Products(Id, Name, Price) VALUES
                                    ('1E7F21A1-237B-427B-BDED-1B1B32639E48','Samsung galaxy s24 Ultra',20000.00),
                                    ('ADE91C53-C314-4CFC-8B4A-D24F19E35646','iPhone 15 pro',30000.00),
                                    ('BC1FA982-51DD-41E9-8130-D7F0C08E07B7','Samsung s26',45000.00),
                                    ('E3AD30F6-5FD7-4F58-8EAD-1B5E964750DA','OpePlus Pro',12000.00),
                                    ('80AE1DB7-097B-4DE1-BB23-36A08E49A825','Google Pixel 8 Pro',8000.00)";
                cmd.CommandText = sqlSeed;
                cmd.ExecuteNonQuery();
            }
        }

        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            string sql = "SELECT * FROM Products";

            using var cmd = new MySqlCommand(sql, connection);
            try
            {
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(Product.FromReader(reader));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: {0}", ex.Message);
            }
            return products;
        }

        // Інші методи друку залишаються такими ж
        public void PrintNP()
        {
            foreach (var p in GetProducts().OrderBy(p => p.Price))
                Console.WriteLine("{0}) -- {1:F2}", p.Name, p.Price);
        }

        public void PrintDesc()
        {
            foreach (var p in GetProducts().OrderByDescending(p => p.Price))
                Console.WriteLine("{0}) -- {1:F2}", p.Name, p.Price);
        }

        public void PrintAlphabet()
        {
            foreach (var p in GetProducts().OrderBy(p => p.Name))
                Console.WriteLine("{0}) -- {1:F2}", p.Name, p.Price);
        }

        public void PrintLoser()
        {
            foreach (var p in GetProducts().OrderBy(p => p.Price).Take(3))
                Console.WriteLine("{0}) -- {1:F2}", p.Name, p.Price);
        }

        public void PrintTop()
        {
            foreach (var p in GetProducts().OrderByDescending(p => p.Price).Take(3))
                Console.WriteLine("{0}) -- {1:F2}", p.Name, p.Price);
        }

        public void PrintRandom()
        {
            var rnd = new Random();
            var randomProducts = GetProducts().OrderBy(p => rnd.Next()).Take(3).ToList();
            foreach (var p in randomProducts)
                Console.WriteLine("{0}) -- {1:F2}", p.Name, p.Price);
        }
    }
}
