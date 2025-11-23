using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Events.Delegate
{
    internal class PriceView
    { 
        public PriceView()
        {
            Console.WriteLine("PriceView initial price: " + State.Instance.Price);
            State.Instance.AddListener(_listener);
        }
        private readonly StateListener _listener = new StateListener ((_price)=>
        {
            Console.WriteLine("PriceView detected new price: " + _price);
        });
        ~PriceView() //finalizer
        {
            State.Instance.RemoveListener(_listener);
        }
    }
}
