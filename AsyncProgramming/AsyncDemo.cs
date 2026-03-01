using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sharp_231.AsyncProgramming
{
    internal class AsyncDemo
    {
        private readonly Random rand = new();
        private readonly List<int> randomArr = new();
        private int _completedCount = 0;
/* 
        public void Run()
        {
            Console.WriteLine("AsyncDemo start");
          
            randomArr.Clear();
            _completedCount = 0;
            
            RunAsyncHomework().Wait();
            Console.WriteLine("AsyncDemo finish");
        }

        private async Task RunAsyncHomework()
        {
            Console.Write("> введіть кількість чисел для генерування: ");
            if (!int.TryParse(Console.ReadLine(), out int amount) || amount <= 0) return;

            List<Task> tasks = new();
            for (int i = 0; i < amount; i++)
            {
              
                tasks.Add(GenerateAndAddElement(amount));
            }

           
            await Task.WhenAll(tasks);
        }

        private async Task GenerateAndAddElement(int total)
        {
           
            await Task.Delay(rand.Next(500, 2000));

            int value = rand.Next(100);

         
            lock (randomArr)
            {
                randomArr.Add(value);
                _completedCount++;

               
                Console.WriteLine($"[{string.Join(", ", randomArr)}]");

             
                if (_completedCount == total)
                {
                    Console.WriteLine($"\nРезультат: [{string.Join(", ", randomArr)}]");
                }
            }
        }
        */
   
   public async Task Run()
   {
       Console.WriteLine("--- ДЗ: Async-Await формування масиву ---");
       Console.Write("> введіть кількість чисел для генерування: ");
            
       if (!int.TryParse(Console.ReadLine(), out int amount) || amount <= 0)
       {
           Console.WriteLine("Некоректна кількість.");
           return;
       }

       randomArr.Clear();
       _completedCount = 0;

       // Створюємо список задач
       List<Task> tasks = new();
       for (int i = 0; i < amount; i++)
       {
           tasks.Add(GenerateElementAsync(amount));
       }

       // Очікуємо завершення всіх задач паралельно
       await Task.WhenAll(tasks);
            
       Console.WriteLine("Процес завершено.");
   }

   private async Task GenerateElementAsync(int total)
   {
       // Імітуємо затримку (від 0.5 до 2 секунд)
       await Task.Delay(rand.Next(500, 2000));

       int generated = rand.Next(100);

       // Синхронізація доступу до спільної колекції
       lock (randomArr)
       {
           randomArr.Add(generated);
           _completedCount++;

           // Вивід проміжного результату
           Console.WriteLine($"[{string.Join(", ", randomArr)}]");

           // Якщо це остання задача — виводимо фінальний результат
           if (_completedCount == total)
           {
               Console.WriteLine($"\nРезультат: [{string.Join(", ", randomArr)}]");
           }
       }
   }
}
}