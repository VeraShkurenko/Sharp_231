using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Events.Actions
{
    internal class ProductPage
    {
        public ProductPage()
        {
            Subject.Instance.Subscribe(OnPriceChanged);
            Console.WriteLine("Products initial price: " + Subject.Instance.Price);
        }
        private void OnPriceChanged()
        {
            Console.WriteLine("Product detected new price: " + Subject.Instance.Price);
        }
        ~ProductPage() //finalizer
        {
            Subject.Instance.Unsubscribe(OnPriceChanged);
        }
    }
}
