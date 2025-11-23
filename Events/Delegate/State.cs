using Sharp_231.Events.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Events.Delegate
{
    //делегат - тип данних для методів (аналог action<double>)
    public delegate void StateListener(double price);
    internal class State
    {
        private static State? _instance = null;
        public static State Instance => _instance ??= new(0.0);
        public State(double initialPrice)
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("Delegate instance already exists.");
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
                NotifyListeners(_price);
            }
        }


        private readonly List<StateListener> listeners = [];

        public void AddListener(StateListener listener) => listeners.Add(listener);

        public void RemoveListener(StateListener listener) => listeners.Remove(listener);

        private void NotifyListeners(double price) =>
            listeners.ForEach(listener => listener(price));
    }

}
