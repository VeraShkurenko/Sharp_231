using Sharp_231.Data.Dto;
using Sharp_231.Events;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Sharp_231.Data
{
    internal class DataDemo
    {
        public void Run()
        {
            string connectionString = "Server=127.0.0.1;Port=3306;Database=demo;User Id=root;Password=veravira;";

            using var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Connection failed: " + ex.Message);
                return;
            }

            DataAccessor dataAccessor = new();
            // dataAccessor.Install();
            // dataAccessor.Seed();
            List<Product> products = dataAccessor.GetProducts();

            // products.ForEach(Console.WriteLine);
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

            using var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Connection failed: " + ex.Message);
                return;
            }

            string sql = "SELECT NOW();"; // аналог CURRENT_TIMESTAMP у MySQL
            using var cmd = new MySqlCommand(sql, connection);
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
        }
    }
}
/*
 робота з даними на прикладі БД
БД зазвичай є відокремленим від проєкту сервісом, що мимагає окремого
підключення та спесифічної взаємодії.

LocalDB New Item - Sercice DB - Create
знаходимо рдок підключення до БД через її властивості у Server Explorer 

NuGet - система управлінння підключенними додатковими модулями (бібліотеками)
проєкт  C#.NET:Tools -- NuGet Package Manager - manage 
Microsoft.Data.sqlClient - додаткові інструменти
для взаємодії з субд MS SQLServer у т.ч LocacDB

ORM - Object Relation Mapping - відображення даних та їх зв'язків на об'єкти(мови програмування) та їх зв'язки
DTO - Data transfer Object(Entity) - об'єкти (класи) для представлення даних
DAO - Data Access Object - об'єкти (класи) для оперування з DTO
 */

/*
 Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\arina\source\repos\Sharp_231\Data\Database1.mdf;Integrated Security=True
 */