using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.AsyncProgramming
{
    internal class ThreadJoin
    {
        Random rand = new();
        List<int> randomArr = new();

        public void Run()
        {
            Console.WriteLine("{0:F2} Array initialized", (DateTime.Now.Ticks % (long)1e8) / 1e7);
            Console.Write("Введіть кількість елементів для генерування: ");
            int amount = int.Parse(Console.ReadLine()!);
            List<Thread> threads = new();
            for (int i = 0; i < amount; i++)
            {
                Thread thread = new(AddRandomElement);
                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
            Console.WriteLine("{0:F2} Array generation finished", (DateTime.Now.Ticks % (long)1e8) / 1e7);

            Console.Write("Результат: [");
            for (int i = 0; i < randomArr.Count; i++)
            {
                Console.Write(i == randomArr.Count - 1 ? randomArr[i] : randomArr[i] + ", ");
            }
            Console.WriteLine("]");
        }

        private void AddRandomElement()
        {
            Console.WriteLine("{0:F2} Started generating element", (DateTime.Now.Ticks % (long)1e8) / 1e7);
            Thread.Sleep(1000);
            int generated;
            lock (randomArr)
            {
                generated = rand.Next(100);
                randomArr.Add(generated);
            }
            Console.WriteLine("{0:F2} Finished generating element with value {1}", (DateTime.Now.Ticks % (long)1e8) / 1e7, generated);
        }


        public void Run2()
        {
            Console.WriteLine("{0:F2} Breakfast start", (DateTime.Now.Ticks % (long)1e8) / 1e7);

            Thread makeCoffee = new(MakeCoffee);
            makeCoffee.Start();
            Thread roastBacon = new(RoastBacon);
            roastBacon.Start();
            Thread makeToast = new(MakeToast);
            makeToast.Start();
            
            makeToast.Join();           //Не ефективно запускати і одразу чекати. ( Start() => Join() )
            roastBacon.Join();
            makeCoffee.Join();

            Console.WriteLine("{0:F2} Breakfast finish", (DateTime.Now.Ticks % (long)1e8) / 1e7);
        }

        private void MakeToast()
        {
            Console.WriteLine("{0:F2} MakeToast Start", (DateTime.Now.Ticks % (long)1e8) / 1e7);
            Thread.Sleep(100);
            Console.WriteLine("{0:F2} MakeToast Finish", (DateTime.Now.Ticks % (long)1e8) / 1e7);
        }

        private void RoastBacon()
        {
            Console.WriteLine("{0:F2} RoastBacon Start", (DateTime.Now.Ticks % (long)1e8) / 1e7);
            Thread.Sleep(300);
            Console.WriteLine("{0:F2} RoastBacon Finish", (DateTime.Now.Ticks % (long)1e8) / 1e7);
        }

        private void MakeCoffee()
        {
            Console.WriteLine("{0:F2} MakeCoffee Start", (DateTime.Now.Ticks % (long)1e8) / 1e7);
            Thread.Sleep(1000);
            Console.WriteLine("{0:F2} MakeCoffee Finish", (DateTime.Now.Ticks % (long)1e8) / 1e7);
        }
    }
}
