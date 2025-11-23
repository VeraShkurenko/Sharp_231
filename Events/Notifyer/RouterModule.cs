using SharpKnP321.Events.Notifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Events.Notifyer
{
    internal class RouterModule
    {
        private readonly ChangeListener listener;

        public RouterModule()
        {
            listener = (prop) =>
            {
                if (prop == nameof(GlobalState.Instance.ActivePage))
                    Console.WriteLine($"RouterModule: new page - {GlobalState.Instance.ActivePage}");

                if (prop == nameof(GlobalState.Instance.LastSyncMoment))
                    Console.WriteLine($"RouterModule: synced at {GlobalState.Instance.LastSyncMoment}");
            };

            Console.WriteLine($"RouterModule initial page: {GlobalState.Instance.ActivePage}");
            Position.Instance.Subscribe(listener);
        }


        ~RouterModule()
        {
            Position.Instance.Unsubscribe(listener);
        }
    }
}
