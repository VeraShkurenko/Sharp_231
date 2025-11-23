using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Events.Observer
{
    internal class ObserverDemo
    {
        public void Run()
        {
            Console.WriteLine("\n     Observer Demo");
            PriceWidget pw = new();
            CartWidget cw = new();
            ProductWidget prw = new();
            DiscountWidget dw = new();
            Console.WriteLine("---------------------");

            Publisher.Instance.Price =200.0;
        }
    }
}
