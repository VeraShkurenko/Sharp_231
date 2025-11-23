using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Events.Actions
{
    internal class PricePage
    {
        public PricePage()
        {
            Subject.Instance.Subscribe (OnPriceChanged);
            Console.WriteLine("PricePage initial price: " + Subject.Instance.Price);
        }
        private void OnPriceChanged()
        {
            Console.WriteLine("PricePage detected new price: " + Subject.Instance.Price);
        }
        ~PricePage() //finalizer
        {
            Subject.Instance.Unsubscribe(OnPriceChanged);
        }
    }
}
