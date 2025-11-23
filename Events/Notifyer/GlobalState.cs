using Sharp_231.Events.Notifyer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpKnP321.Events.Notifier
{
    internal class GlobalState
    {
        private static GlobalState? _instance = null;
        public static GlobalState Instance => _instance ??= new();

        private GlobalState() { }
        public double Price { get; set; } = 100.0;
        public DateTime LastSyncMoment { get; set; }
        public String? UserName { get; set; } = "Admin";
        public String ActivePage { get; set; } = "Home";

        public void SetPrice(double value)
        {
            Price = value;
            Position.Instance.Notify(nameof(Price));
        }

        public void SetUser(string user)
        {
            UserName = user;
            Position.Instance.Notify(nameof(UserName));
        }

        public void SetPage(string page)
        {
            ActivePage = page;
            Position.Instance.Notify(nameof(ActivePage));
        }

        public void SetSync(DateTime moment)
        {
            LastSyncMoment = moment;
            Position.Instance.Notify(nameof(LastSyncMoment));
        }
    }
}

/* Формалізм ChangeNotifier передбачає запуск підписників з передачею
 * до них імені поля/властивості, що зазнало зміни.
 * Ось текст зі зображення, переписаний точно:

* Наприклад, на сторінці Price потрібна інформація про ціну,
* на сторінці Router — про ActivePage
* на сторінці Profile — про UserName
* на всіх сторінках — LastSyncMoment

 */
