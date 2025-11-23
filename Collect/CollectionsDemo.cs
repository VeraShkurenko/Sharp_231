using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sharp_231.Collection
{
    internal class CollectionsDemo
    {
        class Manager
        {
            public int Id { get; set; }
            public String Name { get; set; }
            public override string ToString()
            {
                return $"{Name}(id ={Id})";
            }
        }
        class Sale
        {
            public int Id { get; set; }
            public int ManagerId { get; set; }
            public double Price { get; set; }
            public override string ToString()
            {
                return $"Sale(id = {Id}), ManagerId ={ManagerId}, Price = {Price:F2}";
            }
        }
        public void Run()
        {
            Random random = new();
            List<Manager> managers = [
            new() { Id=1,Name="John Doe"},
                new() { Id=2,Name="July Smith"},
                new() { Id=3,Name="Nik Forest"},
                new() { Id=4,Name="Barbara White"},
                new() { Id=5,Name="Will Black"},
                ];
            List<Sale> sales = [..Enumerable
                    .Range(1,100)
                    .Select (x => new Sale{
                        Id =x,
                        ManagerId = 1+x% managers.Count,
                        Price = random.NextDouble()*1000
                    })
                ];

            //    sales.ForEach(Console.WriteLine);
            //     sales.Take(10).ToList().ForEach(Console.WriteLine);

            // Підєднати дані – вивести Менеджер–сума продажу
            var query1 = sales
                .Select(s => new
                {
                    Id = s.ManagerId,
                    managers.First(m => m.Id == s.ManagerId).Name,
                    s.Price
                });
            //foreach (var item in query1)
            //{
            //    Console.WriteLine("{0}-{1} {2:F2}",item.Id, item.Name, item.Price);
            //}

            var query2 = sales
                .Join(                  //join - поєднання колекцій
                managers,               // інша колекція
                s => s.ManagerId,       // вибір зовнішнього ключа
                m => m.Id,              //вибір внутрішнього ключа
                (s, m) => new           //правило комбінації пари-вибірки
                {
                    Sale = s,
                    Manager = m
                });
            //foreach (var item in query2)
            //{
            //    Console.WriteLine("{0} - {1}", item.Manager, item.Sale);
            //}

            var query3 = managers
                .GroupJoin(                   //Поєднання з групуванням
                sales,                        //(в одного менеджера є кілька продажів)
                m => m.Id,
                s => s.ManagerId,
                (m, ss) => new                //м - один менеджер
                {                             //сс - множина продажів по даному менеджеру
                    Manager = m,
                    Sum = ss.Sum(sales => sales.Price)
                })
                .Select(item => new
                {
                    Name = item.Manager.Name,
                    Total = item.Sum
                })
                .OrderByDescending(item => item.Total);

            foreach (var item in query3)
            {
                Console.WriteLine("{0} -- {1:F2}", item.Name, item.Total);
            }
            Console.WriteLine("=======================");
            var salesMore500 = sales
              .Join(
                  managers,
                  s => s.ManagerId,
                  m => m.Id,
                  (s, m) => new
                  {
                      Manager = m.Name,
                      s.Price
                  }
              )
              .Where(x => x.Price > 800)
              .OrderBy(x => x.Price);

            Console.WriteLine("Sales > 800:");
            foreach (var item in salesMore500)
            {
                Console.WriteLine("{0} -- {1:F2}", item.Manager, item.Price);
            }
            Console.WriteLine("=======================");

            var salesLess100 = sales
    .Join(
        managers,
        s => s.ManagerId,
        m => m.Id,
        (s, m) => new
        {
            Manager = m.Name,
            s.Price
        }
    )
    .Where(x => x.Price < 100)
    .OrderBy(x => x.Price);

            Console.WriteLine("Sales < 100:");
            foreach (var item in salesLess100)
            {
                Console.WriteLine("{0} -- {1:F2}", item.Manager, item.Price);
            }
            Console.WriteLine("=======================");

            double[] Grant = { 0.10, 0.07, 0.05 };
            var top3 = query3.Take(3).ToList();

            for (int i = 0; i < top3.Count; i++)
            {
                var w = top3[i];
                var g = w.Total * Grant[i];  
                double pr = Grant[i]*100;
                Console.WriteLine(
                    "The best result of {0:F2} + the {3:F0}% ({2:F2}) has: {1}",
                    w.Total, w.Name, g,pr
                );
            }


        }
        public void RunL()
        {
            List<String> strings = [];
            for (int i = 0; i < 10; i++)
            {
                strings.Add("String " + i);
            }
            var query =
                from s in strings
                where s[^1] == '2' || s[^1] == '3'
                select s;
            foreach (var str in query)
            {
                Console.WriteLine(str);
            }

            //Method-syntax
            var query2 = strings
                 .Where(s => ((int)s[^1] & 1) == 1)
                 .Select(s => s.ToUpper());

            foreach (string str in query2)
            {
                Console.WriteLine(str);
            }
            //Предикат - в програмування функція або вираз
            //яка приймає логічний результат true/false
        }
        public void RunList()
        {
            List<String> strings = [];
            for (int i = 0; i < 10; i++)
            {
                strings.Add("String " + i);
            }
            foreach (string str in strings)
            {
                Console.WriteLine(str);
                //  strings.Remove(str);  // System.InvalidOperationException: Collection was modified
                //  strings.Add("new");  // Collection was modified
            }
            // strings.Add("new");//поза  циклом помилок немає
            Console.WriteLine("-------------");
            strings.ForEach(Console.WriteLine);

            // Під час ітерації видалення призведе до винятку, відповідно для рішення 
            // потрібна друга колекція
            List<string> removes = new();

            foreach (var str in strings)      // утворюємо цикл по strings та модифікуємо removes
            {
                char c = str[^1];
                if (c <= '9' && c >= '0')
                {
                    int n = (int)c;
                    if ((n & 1) == 1)         // у першому циклі знаходимо ті, що мають 
                    {                         // бути видалені та поміщаємо їх у другу колекцію
                        removes.Add(str);
                    }
                }
                // Console.WriteLine(str[1]);
            }
            // утворюємо цикл по removes та модифікуємо strings
            foreach (string rem in removes)
            {
                strings.Remove(rem);
            }
            Console.WriteLine("-----------");
            strings.ForEach(Console.WriteLine);
            Console.WriteLine("-----------");

        }
        /* bool: a && b
         * bitwise: x & y - побітовий "ТА"
         * x = 10    1010
         * y = 8     1000
         * x & y    
         * 0
        001000 | 001001 = 001001

        Slices
 * str = "The string"
 * str[1] = 'h'
 * str[1..4] = "he s"
 * str[..5] == str[0..5] = "The s"
 * str[5..] = "tring"
 * str[^1] = "g"   (-1 — перший з кінця)
 * str[..^1] = "The strin"
 */

        public void RunArr()
        {
            Console.WriteLine("Collections Demo");
            /* Масив(класично) - спосіб збереження даних, за якого однотипні дані розміщуються у
             * пам'яті послідовно і мають визначений розмір.
             * У C#.NET масив - об'єкт, який забезпечує управління класичним масивом.
             */
            String[] arr1 = new String[3];
            String[] arr2 = new String[3] { "1", "2", "3" };
            String[] arr3 = { "1", "2", "3" };
            String[] arr4 = ["1", "2", "3"];
            arr1[0] = new("Str 1");   // базовий синтаксис роботи з елементами масивів
            arr1[1] = arr2[0];        // забезпечується індексаторами на відміну від С++
                                      // де [n] - це розіменування зі зміщенням: a[n] == *(a + n)
                                      // Dereferencing (Posible null dereferencing -> NullReferenceExc)

            arr1[0] = "New Str 1";
            int x = arr1.Length;

            /* На відміну від масивів колекції:
             * дозволяють змінний розмір
             * дозволяють непослідовне збереження
             */
        }
        /* Garbage Collector
 * [obj1  obj2  obj3 ....]
 * [obj1  ----  obj3 ....] 
 * 
 * 
 *                pointer                      pointer
 *                   |                           |
 * GC: [obj1  ----  obj3 ....] --> [obj1 obj3 ........] 
 *                   |                    |
 *               reference             reference
 * 
 * 
 * [ arr1[0] arr1[1] ...        "Str1" ... "New Str 1" ]
 *      \_x______________________/           /
 *        \_________________________________/
 */
    }
}
