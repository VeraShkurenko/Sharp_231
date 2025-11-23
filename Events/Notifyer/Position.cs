using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Events.Notifyer
{
    public delegate void ChangeListener(string propertyName);

    internal class Position
    {
        private static Position? _instance = null;
        public static Position Instance => _instance ??= new();

        private event ChangeListener? OnChange;

        public void Subscribe(ChangeListener listener) =>
            OnChange += listener;

        public void Unsubscribe(ChangeListener listener) =>
            OnChange -= listener;

        public void Notify(string propertyName) =>
            OnChange?.Invoke(propertyName);
    }
}
