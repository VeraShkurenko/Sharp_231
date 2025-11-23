using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Events.Actions
{
    internal class CartPage
    {
        public CartPage()
        {
            Subject.Instance.Subscribe(OnPriceChanged);
            Console.WriteLine("CartPage initial price: " + Subject.Instance.Price);
        }
        private void OnPriceChanged()
        {
            Console.WriteLine("CartPage detected new price: " + Subject.Instance.Price);
        }
        ~CartPage() //finalizer
        {
            Subject.Instance.Unsubscribe(OnPriceChanged);
        }
    }
}
