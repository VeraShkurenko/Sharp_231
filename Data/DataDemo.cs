using Sharp_231.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using Sharp_231.Attributes;

namespace Sharp_231.Data
{
    internal class DataDemo
    {
        
        public void Run()
        { 
            DataAccessor dataAccessor = new(); 
            
            var (byReceipts, byQuantity, byAmount) = dataAccessor.GetTopSalesTodayTuple();

            Console.WriteLine("=== Top 3 за кількістю чеків ===");
            byReceipts.ForEach(x => Console.WriteLine($"{x.ProductName} -- {x.CountChecks} чеків"));

            Console.WriteLine("=== Top 3 за кількістю штук ===");
            byQuantity.ForEach(x => Console.WriteLine($"{x.ProductName} -- {x.TotalQuantity} шт."));

            Console.WriteLine("=== Top 3 за сумою продажів ===");
            byAmount.ForEach(x => Console.WriteLine($"{x.ProductName} -- {x.TotalSum:F2} грн"));
            
            

            //dataAccessor.Install();
            //dataAccessor.Seed();
                      
           /* var accesses = dataAccessor.GetAll<Access>();
            var managers = dataAccessor.GetAll<Manager>();

            var result = accesses.Join(
                managers,
                a => a.ManagerId,
                m => m.Id,
                (a, m) => new
                {
                    ManagerName = m.Name,
                    Login = a.Login
                });

            foreach (var item in result)
            {
                Console.WriteLine($"{item.ManagerName} -- {item.Login}");
            } */
 
  
            //   dataAccessor.Install();
            //   dataAccessor.Seed(); 
            

            /*  Console.Write("Введіть номер місяця (1-12): ");
            int month = int.Parse(Console.ReadLine());

            var (curr, prev) = dataAccessor.GetSalesInfoByMonth(month);

            Console.WriteLine($"Продажі за {month}-й місяць:");
            Console.WriteLine($"Поточний рік: {curr}");
            Console.WriteLine($"Попередній рік: {prev}"); */
            
           /* Console.Write("Введіть номер місяця (1-12): ");
            int month = int.Parse(Console.ReadLine());
            int year = DateTime.Now.Year;

            Console.WriteLine("=== Через ORM (LINQ) ===");
            var statsOrm = dataAccessor.GetSalesStatsByMonth(month, year);
            foreach (var s in statsOrm)
                Console.WriteLine($"{s.ProductName} -- {s.Quantity}");

            Console.WriteLine("=== Через прямий SQL ===");
            var statsSql = dataAccessor.GetSalesStatsByMonthSQL(month, year);
            foreach (var s in statsSql)
                Console.WriteLine($"{s.ProductName} -- {s.Quantity}"); */

        }


        public void Run2()
        {
            DataAccessor dataAccessor = new();
            var db = new DataAccessor();
            db.SeedSalesToday();      // додаємо тестові продажі на сьогодні
            db.GetTopSalesToday();    // виводимо топ-3


            Console.WriteLine("=== Departments (EnumAll) ===");
            foreach (var dep in dataAccessor.EnumAll<Department>())
                Console.WriteLine(dep);

            Console.WriteLine("=== Products (EnumAll) ===");
            foreach (var p in dataAccessor.EnumAll<Product>())
                Console.WriteLine(p);

            Console.WriteLine("=== Managers (EnumAll) ===");
            foreach (var m in dataAccessor.EnumAll<Manager>())
                Console.WriteLine(m);
            
            Console.WriteLine("=== News ===");
            foreach (var n in dataAccessor.EnumNews())
            {
                Console.WriteLine($"{n.Title} | {n.Moment}");
            }

        }
        
        

        public void Run3()
        {

            DataAccessor dataAccessor = new();
            dataAccessor.Install();
            dataAccessor.Seed();

            List<Department> departments = dataAccessor.GetAll<Department>();
            List<Manager> managers = dataAccessor.GetAll<Manager>();
            
                //LINQ - робота з колекціями за аналогією з БД
                
                
            
                var result = managers.Join(
                    departments,          
                    m => m.DepartmentId,   
                    d => d.Id,           
                    (m, d) => new           
                    {
                        ManagerName = m.Name,
                        DepartmentName = d.Name
                    });

                foreach (var item in result)
                {
                    Console.WriteLine($"{item.ManagerName} -- {item.DepartmentName}");
                }
                
                var result2 = departments
                    .GroupJoin(
                        managers,
                        d => d.Id,
                        m => m.DepartmentId,
                        (d, mgrs) => new
                        {
                            DepartmentName = d.Name,
                            ManagerCount = mgrs.Count(),
                            ManagerNames = string.Join(", ", mgrs.Select(m => m.Name))
                        });

                foreach (var item in result2)
                {
                    Console.WriteLine($"{item.DepartmentName} -- {item.ManagerCount} -- {item.ManagerNames}");
                }

                    
                

            /*PrintTable<Product>(dataAccessor, "===========PRODUCTS===========");
            PrintTable<Department>(dataAccessor, "==========DEPARTMENTS==========");
            PrintTable<Manager>(dataAccessor, "=========MANAGERS===========");
            PrintTable<News>(dataAccessor, "=========NEWS===========");*/

            /*   dataAccessor.GetAll<Product>().ForEach(Console.WriteLine);
               Console.WriteLine("-----------------");

               dataAccessor.GetAll<Department>().ForEach(Console.WriteLine);
               Console.WriteLine("-----------------");

               dataAccessor.GetAll<Manager>().ForEach(Console.WriteLine);
               Console.WriteLine("-----------------");

               dataAccessor.GetAll<News>().ForEach(Console.WriteLine);
               Console.WriteLine("-----------------"); */


            /* Console.WriteLine("====================");
        dataAccessor.PrintNP();
        Console.WriteLine("====================");
        dataAccessor.PrintDesc();
        Console.WriteLine("====================");
        dataAccessor.PrintAlphabet();
        Console.WriteLine("=========TOP========");
        dataAccessor.PrintTop();
        Console.WriteLine("========WORST=======");
        dataAccessor.PrintLoser();*/
        }

        private void PrintTable<T>(DataAccessor accessor, string title)
        {
            Console.WriteLine(title);
            var items = accessor.GetAll<T>();
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }
    }
}
