using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Events.Observer
{
    internal class DiscountWidget
    {
        private readonly DiscountWidgetSubscriber subscriber;
        public DiscountWidget()
        {
            subscriber = new(OnPriceChanged);

            Publisher.Instance.Subscribe
           (new DiscountWidgetSubscriber(OnPriceChanged));
            double d = Publisher.Instance.Price * 0.1;
            Console.WriteLine("Your discount is: "
              + d);
        }
        public void OnPriceChanged(double price)
        {
            Console.WriteLine("Your discount is set to: " + price*0.1);
        }
        ~DiscountWidget() //finalizer
        {
            Publisher.Instance.Unsubscribe(subscriber);
        }
    }

    public class DiscountWidgetSubscriber(Action<double> action) : ISubscriber
    {
        public void Update(double newPrice)
        {
            action(newPrice);
        }
    }
}
