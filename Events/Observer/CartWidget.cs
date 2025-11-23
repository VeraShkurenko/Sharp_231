using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Events.Observer
{
    internal class CartWidget
    {
        private readonly CartWidgetSubscriber subscriber;
        public CartWidget()
        {
            subscriber = new(OnPriceChanged);
            Publisher.Instance.Subscribe
           (new CartWidgetSubscriber(OnPriceChanged));
            Console.WriteLine("CartWidget subscribed to price "
                +Publisher.Instance.Price);
        }
        public void OnPriceChanged(double price)
        {
            Console.WriteLine("CartWidget: set new price " + price);
        }

        ~CartWidget() //finalizer
        {
            Publisher.Instance.Unsubscribe(subscriber);
        }
    }

    public class CartWidgetSubscriber(Action<double> action) : ISubscriber
    {
        public void Update(double newPrice)
        {
            action(newPrice);
        }
    }
}
