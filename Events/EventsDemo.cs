using Sharp_231.Events.Actions;
using Sharp_231.Events.Delegate;
using Sharp_231.Events.Notifyer;
using Sharp_231.Events.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Events
{
    internal class EventsDemo
    {
        public void Run()
        {
            Console.WriteLine("     Events Demo");
            try
            {

                //new ObserverDemo().Run();

                //new ActionsDemo().Run();

                //new DelegateDemo().Run();

                //Console.WriteLine("\n     Emmiter Demo");

                //Emitter emitter = new(initialPrice: 100.0);
                //PriceComponent pc = new();
                //CartComponent cc = new();
                //ProductComponent prc = new();
                //Console.WriteLine("---------------------");
                //emitter.Price = 200.0;
                new NotifyerDemo().Run();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Не оброблений у програмі виняток: " + ex.ToString());
            }
        }
    }

    public delegate void EmitterListener(double price);

    class Emitter
    {
        private static Emitter? _instance = null;
        public static Emitter Instance => _instance ??= new(0.0);
        public Emitter(double initialPrice)
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("Emitter instance already exists.");
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
                EmitterEvent?.Invoke(_price);
            }
        }

        // Event – синтаксичний "цукор" над колекцією підписників
        private event EmitterListener? EmitterEvent;   // типізація за делегатом

        public void Subscribe(EmitterListener listener) =>
            EmitterEvent += listener;  // підписка – оператор "+="

        public void UnSubscribe(EmitterListener listener) =>
            EmitterEvent -= listener;  // відписка – оператор "-="

        // + автоматичний контроль за NULL значенням – коли колекція
        // підписників порожня, подія набуває значення NULL, з
        // додаванням підписників – утворюється автоматично


    }

    class PriceComponent
    {
        private readonly EmitterListener _listener = (price) =>
        {
            Console.WriteLine("PriceComponent got new price " + price);
        };

        public PriceComponent()
        {
            Console.WriteLine("PriceComponent starts with price " + Emitter.Instance.Price);
            Emitter.Instance.Subscribe(_listener);
        }

        ~PriceComponent()
        {
            Emitter.Instance.UnSubscribe(_listener);
        }

    }
    class CartComponent
    {
        private readonly EmitterListener _listener = (price) =>
        {
            Console.WriteLine("CartComponent got new price " + price);
        };

        public CartComponent()
        {
            Console.WriteLine("CartComponent starts with price " + Emitter.Instance.Price);
            Emitter.Instance.Subscribe(_listener);
        }

        ~CartComponent()
        {
            Emitter.Instance.UnSubscribe(_listener);
        }

    }
    class ProductComponent
    {
        private readonly EmitterListener _listener = (price) =>
        {
            Console.WriteLine("Product got new price " + price);
        };

        public ProductComponent()
        {
            Console.WriteLine("Product starts with price " + Emitter.Instance.Price);
            Emitter.Instance.Subscribe(_listener);
        }

        ~ProductComponent()
        {
            Emitter.Instance.UnSubscribe(_listener);
        }

    }
}
