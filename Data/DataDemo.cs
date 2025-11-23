using Sharp_231.Data.Dto;
using System;
using System.Collections.Generic;

namespace Sharp_231.Data
{
    internal class DataDemo
    {
        public void Run()
        {
            DataAccessor dataAccessor = new();

            var salesSql = dataAccessor.MonthlySalesByProductsSql(2025, 1);
            Console.WriteLine("Продажі за січень 2025 (SQL):");
            salesSql.ForEach(Console.WriteLine);

            Console.WriteLine("---------------");

            // ORM-підрахунок
            var salesOrm = dataAccessor.MonthlySalesByProductsOrm(2025, 1);
            Console.WriteLine("Продажі за січень 2025 (ORM):");
            salesOrm.ForEach(Console.WriteLine);

            Console.WriteLine("====================");
            dataAccessor.PrintNP();
            Console.WriteLine("====================");
            dataAccessor.PrintDesc();
            Console.WriteLine("====================");
            dataAccessor.PrintAlphabet();
            Console.WriteLine("=========TOP========");
            dataAccessor.PrintTop();
            Console.WriteLine("========WORST=======");
            dataAccessor.PrintLoser();
            Console.WriteLine("=======RANDOM=======");
            dataAccessor.PrintRandom();
        }

        public void Run1()
        {
            Console.WriteLine("Data Demo");

            string connectionString = "Server=127.0.0.1;Port=3306;Database=demo;User Id=root;Password=veravira;";
            using var connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine("Connection failed: " + ex.Message);
                return;
            }

            string sql = "SELECT NOW();"; 
            using var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, connection);
            object scalar;
            try
            {
                scalar = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Query failed: " + ex.Message);
                return;
            }

            DateTime timestamp = Convert.ToDateTime(scalar);
            Console.WriteLine("Res: {0}", timestamp);

            connection.Close();
        }
    }
}
