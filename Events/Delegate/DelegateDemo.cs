using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Events.Delegate
{
    internal class DelegateDemo
    {
        public void Run()
        {
            Console.WriteLine("\n     Delegate Demo");
            State state = new State(100.0);
            new PriceView();
            new CartView();
            new ProductView();
            Console.WriteLine("---------------------");
            state.Price = 200.0;
        }
    }
}
