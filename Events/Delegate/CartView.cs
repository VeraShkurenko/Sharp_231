using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Events.Delegate
{
    internal class CartView
    {
        public CartView()
        {
            Console.WriteLine("CartView initial price: " + State.Instance.Price);
            State.Instance.AddListener(_listener);
        }
        private readonly StateListener _listener = new StateListener((_price) =>
        {
            Console.WriteLine("CartView detected new price: " + _price);
        });
        ~CartView() //finalizer
        {
            State.Instance.RemoveListener(_listener);
        }
    }
}
