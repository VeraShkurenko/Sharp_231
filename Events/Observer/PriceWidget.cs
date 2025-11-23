using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Events.Observer
{
    internal class PriceWidget
    {
        private readonly PriceWidgetSubscriber subscriber;
        public PriceWidget()
        {
            subscriber = new(OnPriceChanged);

            Publisher.Instance.Subscribe
           (new PriceWidgetSubscriber(OnPriceChanged));
            Console.WriteLine("PriceWidget subscribed to price "
              + Publisher.Instance.Price);
        }
        public void OnPriceChanged(double price)
        {
            Console.WriteLine("PriceWidget: set new price " + price);
        }
        ~PriceWidget() //finalizer
        {
            Publisher.Instance.Unsubscribe(subscriber);
        }
    }

    public class PriceWidgetSubscriber(Action<double> action) : ISubscriber
    {
        public void Update(double newPrice)
        {
            action(newPrice);
        }
    }

}
