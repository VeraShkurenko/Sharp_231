using SharpKnP321.Events.Notifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Events.Notifyer
{
    internal class PricePage
    {
        private readonly ChangeListener listener;

        public PricePage()
        {
            listener = (prop) =>
            {
                if (prop == nameof(GlobalState.Instance.Price))
                    Console.WriteLine($"PricePage: new price {GlobalState.Instance.Price}");

                if (prop == nameof(GlobalState.Instance.LastSyncMoment))
                    Console.WriteLine($"PricePage: synced at {GlobalState.Instance.LastSyncMoment}");
            };

            Console.WriteLine($"PricePage initial price: {GlobalState.Instance.Price}");
            Position.Instance.Subscribe(listener);
        }


        ~PricePage()
        {
            Position.Instance.Unsubscribe(listener);
        }
    }
}
