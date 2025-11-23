using MySql.Data.MySqlClient;
using System;

namespace Sharp_231.Data.Dto
{
    internal class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public double Price { get; set; }

        public static Product FromReader(MySqlDataReader reader)
        {
            return new Product
            {
                Id = Guid.Parse(reader["Id"].ToString()),
                Name = reader["Name"].ToString(),
                Price = Convert.ToDouble(reader["Price"])
            };
        }

        public override string ToString()
        {
            return $"{Id.ToString()[..3]}...{Id.ToString()[^3..]} - {Name}  {Price:F2}";
        }
    }
}
 /*
 DTO - Data transfer Object(Entity) - об'єкти (класи) для представлення даних
  Відображення рядка таблиці БД (Products)
 
  */
/*
DTO - Data transfer Object(Entity) - об'єкти (класи) для представлення даних
 Відображення рядка таблиці БД (Products)

 */