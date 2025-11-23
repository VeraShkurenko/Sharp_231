using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Events.Actions
{
    internal class Subject
    {
        private static Subject? _instance = null;
        public static Subject Instance => _instance ??= new(0.0);
        public Subject(double initialPrice)
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("Singleton instance already exists.");
            }
            _price = initialPrice;
            _instance = this;
        }
        private double _price;
        public double Price
        {
            get => _price;
            set
            {
                _price = value;
                NotifySubscribers();
            }
        }

        private readonly List<Action> subscribers = [];
        public void Subscribe(Action action)
        {
            lock (subscribers)
            {
                subscribers.Add(action);
            }
        }
        public void Unsubscribe(Action action)
        {
            lock (subscribers)
            {
                subscribers.Remove(action);
            }
        }
        private void NotifySubscribers()
        {
            lock (subscribers)
            {
                subscribers.ForEach(s => s.Invoke());
            }
        }

    }
}
