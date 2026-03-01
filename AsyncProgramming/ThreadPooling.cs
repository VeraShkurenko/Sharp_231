using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.AsyncProgramming
{
    internal class ThreadPooling
    {

        Random rand = new();
        List<int> randomArr = new();

        public void Run()
        {
            Console.WriteLine("ThreadPooling start");
            Console.Write("Введіть кількість елементів для генерування: ");
            int amount = int.Parse(Console.ReadLine()!);
            List<Thread> threads = new();
            for (int i = 0; i < amount; i++)
            {
                ThreadPool.QueueUserWorkItem(AddRandomElement, i);
            }
            Thread.Sleep(1500);
            Console.WriteLine("ThreadPooling finish");

            Console.Write("Результат: [");
            for (int i = 0; i < randomArr.Count; i++)
            {
                Console.Write(i == randomArr.Count - 1 ? randomArr[i] : randomArr[i] + ", ");
            }
            Console.WriteLine("]");
        }

        private void AddRandomElement(Object? n)
        {
            Console.WriteLine("{0:F2} Started generating element #{1}", (DateTime.Now.Ticks % (long)1e8) / 1e7, n);
            Thread.Sleep(1000);
            int generated;
            lock (randomArr)
            {
                generated = rand.Next(100);
                randomArr.Add(generated);
            }
            Console.WriteLine("{0:F2} Finished generating element #{1} with value {2}", (DateTime.Now.Ticks % (long)1e8) / 1e7, n, generated);
        }

        public void Run2()
        {
            ThreadPool.SetMaxThreads(2, 2);
            Console.WriteLine("ThreadPooling start");
            ThreadPool.QueueUserWorkItem(ThreadAction, 1000);
            ThreadPool.QueueUserWorkItem(ThreadAction, 500);
            ThreadPool.QueueUserWorkItem(ThreadAction, 300);
            Thread.Sleep(500);
            Console.WriteLine($"ThreadPooling finish: {ThreadPool.CompletedWorkItemCount} done, {ThreadPool.PendingWorkItemCount} terminated");
        }

        private void ThreadAction(Object? timeout)
        {
            Console.WriteLine($"ThreadAction {timeout} start");
            Thread.Sleep((int)timeout!);
            Console.WriteLine($"ThreadAction {timeout} finish");
        }
    }
}
/*  Thread Pool - пул потоків - середовище виконання потоків з фоновим пріоритетом
 *  Особливість - потоки, що не встигли допрацюваи до завершення головного потоку (Main), скасовуються
 */
