using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Events.Observer
{
    internal class ProductWidget
    {
        private readonly ProductWidgetSubscriber subscriber;
        public ProductWidget()
        {
            subscriber = new(OnPriceChanged);

            Publisher.Instance.Subscribe
           (new ProductWidgetSubscriber(OnPriceChanged));
            Console.WriteLine("Products price: "
              + Publisher.Instance.Price);
        }
        public void OnPriceChanged(double price)
        {
            Console.WriteLine("Products new price: " + price);
        }
        ~ProductWidget() //finalizer
        {
            Publisher.Instance.Unsubscribe(subscriber);
        }
    }

    public class ProductWidgetSubscriber(Action<double> action) : ISubscriber
    {
        public void Update(double newPrice)
        {
            action(newPrice);
        }
    }
}
