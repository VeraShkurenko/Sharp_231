using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Events.Actions
{
    internal class ActionsDemo
    {
        public void Run()
        {
            Console.WriteLine("\n     Actions Demo");
            Subject s = new(initialPrice: 100.0);
            PricePage pp = new();
            CartPage cp = new();
            ProductPage prp = new();
            Console.WriteLine("---------------------");
            s.Price = 200.0;
        }
    }
}
