using SharpKnP321.Events.Notifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Events.Notifyer
{
    internal class NotifyerDemo
    {
        public void Run()
        {
            PricePage pp = new();
            ProfilePage prp = new();
            RouterModule rm = new();

            Console.WriteLine("----------------------");

            GlobalState.Instance.SetPrice(200);
            GlobalState.Instance.SetUser("User1337");
            GlobalState.Instance.SetPage("Profile");
            GlobalState.Instance.SetSync(DateTime.Now);
        }
    }
}
