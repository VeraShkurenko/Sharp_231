using SharpKnP321.Events.Notifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Events.Notifyer
{
    internal class ProfilePage
    {
        private readonly ChangeListener listener;

        public ProfilePage()
        {
            listener = (prop) =>
            {
                if (prop == nameof(GlobalState.Instance.UserName))
                    Console.WriteLine($"ProfilePage: new user - {GlobalState.Instance.UserName}");

                if (prop == nameof(GlobalState.Instance.LastSyncMoment))
                    Console.WriteLine($"ProfilePage: synced at {GlobalState.Instance.LastSyncMoment}");
            };

            Console.WriteLine($"ProfilePage initial user: {GlobalState.Instance.UserName}");
            Position.Instance.Subscribe(listener);
        }


        ~ProfilePage()
        {
            Position.Instance.Unsubscribe(listener);
        }
    }
}
