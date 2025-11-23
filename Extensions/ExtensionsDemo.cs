using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharp_231.Extensions.str;
using Sharp_231.Library;

namespace Sharp_231.Extensions
{
    internal class ExtensionsDemo
    {
        public void Run()
        {
            Console.WriteLine("Extensions Demo!");
            //оголошений окремо клас IntExtension вводить метод px() для int
            Console.WriteLine("margin: "+2.px());
            Console.WriteLine("margin: "+20.percnt());
            Console.WriteLine("price: "+20.127.toMoney());
            Console.WriteLine("price: "+20.1.toMoney());
            Console.WriteLine("the_snake_case".SnakeToCamel());
            var j = new Journal
            {
                Title = "ArgC & ArgV",
                Number = "2(113), 2000",
                Publisher = "https://journals.ua/technology/argc-argv/"
            };
            Console.WriteLine(j.IsDaily() ? "Щоденний" : "Не щоденний");
            var n = new Newspaper
            {
                Title = "Урядовий кур'єр",
                Date = new DateOnly(2025, 10, 23),
                Publisher = "Газета Кабінету Міністрів України"
            };
            Console.WriteLine(n.IsDaily() ? "Щоденна" : "Не щоденна");

            DateTime now = DateTime.Now;
            Console.WriteLine("Зараз: " + now.ToSqlFormat());

            DateTime customDate = new DateTime(2025, 11, 6, 11, 32, 23);
            Console.WriteLine("SQL формат: " + customDate.ToSqlFormat());
        }

    }
    public static class PeriodicExtension
    {
        public static bool IsDaily(this IPeriodic lit)
        {
            return lit.GetPeriod() == "Day";
        }
    }
}
/*
 класи-розширення (extension classes) - існтрументи для розширення функціональності
інших класів(чи інтерфейсів) через оголошення спецільних класів-розширень
 */