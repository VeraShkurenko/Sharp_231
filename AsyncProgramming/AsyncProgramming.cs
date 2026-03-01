using System;
using System.Diagnostics;
using System.IO;

namespace Sharp_231.AsyncProgramming
{
    internal class AsyncProgramming
    {
        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\n--- ДЗ: Керування процесами (macOS) ---");
                Console.WriteLine("1. Список процесів");
                Console.WriteLine("2. Відкрити Блокнот (TextEdit) з файлом demo.txt");
                Console.WriteLine("3. Пошук у браузері (Google)");
                Console.WriteLine("4. Відкрити відео/музику (YouTube)");
                Console.WriteLine("0. Вихід");
                Console.Write("Ваш вибір: ");

                string? input = Console.ReadLine()?.Trim();
                switch (input)
                {
                    case "0": return;
                    case "1": ProcessesDemo(); break;
                    case "2": OpenNotepadWithFile(); break;
                    case "3": ProcessWithParam(); break;
                    case "4": OpenMediaPlayer(); break;
                    default: Console.WriteLine("Невірний вибір."); break;
                }
            }
        }

        // 1. Блокнот з відкриттям заданого файлу
        private void OpenNotepadWithFile()
        {
            // Шлях до вашого файлу demo.txt
            // Якщо файл лежить у папці з кодом, вказуємо відносний шлях
            string filePath = "AsyncProgramming/demo.txt";

            if (File.Exists(filePath))
            {
                Console.WriteLine($"Відкриваємо файл: {filePath}");
                // На macOS команда 'open' відкриває файл у програмі за замовчуванням (TextEdit)
                Process.Start("open", filePath);
            }
            else
            {
                Console.WriteLine("Файл demo.txt не знайдено! Спробуйте створити його.");
            }
        }

        // 2. Браузер з пошуком та відкриттям адреси
        private void ProcessWithParam()
        {
            Console.Write("Що шукаємо в Google? ");
            string? search = Console.ReadLine();
            if (!string.IsNullOrEmpty(search))
            {
                string url = $"https://www.google.com/search?q={Uri.EscapeDataString(search)}";
                // На macOS 'open' автоматично використовує стандартний браузер
                Process.Start("open", url);
            }
        }

        // 3. Музичний або відеопрогравач
        private void OpenMediaPlayer()
        {
            Console.WriteLine("Запускаємо медіаресурс...");
            // Можна вказати пряме посилання на відео або шлях до mp3/mp4 файлу
            string mediaUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ"; 
            Process.Start("open", mediaUrl);
        }

        private void ProcessesDemo()
        {
            var processes = Process.GetProcesses().OrderBy(p => p.ProcessName).Take(10);
            foreach (var p in processes)
            {
                Console.WriteLine($"{p.Id}\t{p.ProcessName}");
            }
        }
    }
}