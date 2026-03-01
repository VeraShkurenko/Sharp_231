using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Sharp_231.AsyncProgramming
{
    internal class AsyncDemo
    {
        Random rand = new();
        List<int> randomArr = new();

        public void Run()
        {
            Console.WriteLine("AsyncDemo start");
            RunAsync2().Wait();
            Console.WriteLine("AsyncDemo finish");
        }

        private async Task RunAsync2()
        {
            long startTime = DateTime.Now.Ticks;
            Console.WriteLine("{0:F2} RunAsync start", ((DateTime.Now.Ticks - startTime) % (long)1e8) / 1e7);

            Console.Write("Введіть кількість елементів для генерування: ");
            int amount = int.Parse(Console.ReadLine()!);
            List<Task<int>> tasks = new();
            for (int i = 0; i < amount; i++)
            {
                Task<int> task = AddRandomElement(i);
                tasks.Add(task);
            }

            foreach (var task in tasks)
            {
                randomArr.Add(await task);
            }

            Console.Write("Результат: [");
            for (int i = 0; i < randomArr.Count; i++)
            {
                Console.Write(i == randomArr.Count - 1 ? randomArr[i] : randomArr[i] + ", ");
            }
            Console.WriteLine("]");

            Console.WriteLine("{0:F2} RunAsync finished", ((DateTime.Now.Ticks - startTime) % (long)1e8) / 1e7);
        }

        private async Task<int> AddRandomElement(int n)
        {
            await Task.Delay(1000);
            int generated = rand.Next(100);
            Console.WriteLine("{0:F2} Finished generating element #{1} with value {2}", (DateTime.Now.Ticks % (long)1e8) / 1e7, n, generated);
            return generated;
        }

        public void Run2()
        {
            Console.WriteLine("AsyncDemo start");
            RunAsync().Wait();
            Console.WriteLine("AsyncDemo finish");
        }

        private async Task RunAsync()
        {
            long startTime = DateTime.Now.Ticks;
            Console.WriteLine("{0:F2} RunAsync start", ((DateTime.Now.Ticks - startTime) % (long)1e8) / 1e7);
            //Console.WriteLine("{1:F1} {0}", await GetStringAsync(100), ((DateTime.Now.Ticks - startTime) % (long)1e8) / 1e7);
            //Console.WriteLine("{1:F1} {0}", await GetStringAsync(50, 500), ((DateTime.Now.Ticks - startTime) % (long)1e8) / 1e7);
            //Console.WriteLine("{1:F1} {0}", await GetStringAsync(70, 700), ((DateTime.Now.Ticks - startTime) % (long)1e8) / 1e7);
            Task<String> t100 = GetStringAsync(100);
            Task<String> t70 = GetStringAsync(70, 700);
            var t50 = GetStringAsync(50, 500);
            Console.WriteLine("{1:F2} {0}", await t100, ((DateTime.Now.Ticks - startTime) % (long)1e8) / 1e7);
            Console.WriteLine("{1:F2} {0}", await t50, ((DateTime.Now.Ticks - startTime) % (long)1e8) / 1e7);
            Console.WriteLine("{1:F2} {0}", await t70, ((DateTime.Now.Ticks - startTime) % (long)1e8) / 1e7);
        }

        private async Task<String> GetStringAsync(int length, int delay = 1000)
        {
            //Task.Delay(1000).Wait();
            await Task.Delay(delay);
            return $"The string of length {length}";
        }
    }
}
