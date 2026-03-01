using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sharp_231.AsyncProgramming
{
    internal class Continuations
    {
        private long startTicks;

        public void Run()
        {
            startTicks = DateTime.Now.Ticks;
            Console.WriteLine(Task.Run(GetString)
                .ContinueWith(t => Spacefy(t.Result))
                .ContinueWith(t => Capitalize(t.Result))
                .ContinueWith(t => Slugify(t.Result, "-"))
                .ContinueWith(t => Invert(t.Result, "-"))
                .ContinueWith(t => Ceasar(t.Result))
                .ContinueWith(t => Hide(t.Result))
                .Result);
        }

        public void RunOptimal()
        {
            startTicks = DateTime.Now.Ticks;
            Task<String> chain1 = Task.Run(Work1).ContinueWith(t => Work3(t.Result));
            Task<String> chain2 = Task.Run(Work2).ContinueWith(t => Work4(t.Result));

            Console.WriteLine("{1:F2} Chain1 res: {0}", chain1.Result, ((DateTime.Now.Ticks - startTicks) % (long)1e8) / 1e7);
            Console.WriteLine("{1:F2} Chain2 res: {0}", chain2.Result, ((DateTime.Now.Ticks - startTicks) % (long)1e8) / 1e7);
        }

        private String Hide(String str, char hidingSymbol = '*')
        {
            Task.Delay(1000).Wait();
            string res = string.Join("-", str.Split('-').Select(word =>
                    word.Length <= 3
                        ? word
                        : word[0] + new string(hidingSymbol, word.Length - 2) + word[^1]
                )
            );
            Console.WriteLine($"Hide: '{str}' -> '{res}'");
            return res;
        }

        private String Ceasar(String str)
        {
            Task.Delay(1000).Wait();
            char[] arr = str.ToCharArray();

            for (int i = 0; i < arr.Length; i++)
            {
                char c = arr[i];

                if (c >= 'A' && c <= 'Z')
                    arr[i] = (char)('A' + (c - 'A' + 3) % 26);
                else if (c >= 'a' && c <= 'z')
                    arr[i] = (char)('a' + (c - 'a' + 3) % 26);
            }

            string res = new string(arr);
            Console.WriteLine($"Invert: '{str}' -> '{res}'");
            return res;
        }

        private String Invert(String str, String glue = "-")
        {
            Task.Delay(1000).Wait();
            String res = String.Join(glue, str.Split(glue).Select(s => new string(s.Reverse().ToArray())));
            Console.WriteLine($"Invert: '{str}' -> '{res}'");
            return res;
        }

        private String Slugify(String str, String glue="")
        {
            Task.Delay(1000).Wait();
            String res = Regex.Replace(str, @"\W+", glue);
            res = Regex.Replace(res, @"^\W+|\W+$", "");
            Console.WriteLine($"Slugify: '{str}' -> '{res}'");
            return res;
        }

        private String Capitalize(String str)
        {
            Task.Delay(1000).Wait();
            String res = String.Join(' ', str.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(s => $"{s[0].ToString().ToUpper()}{s[1..]}"));
            Console.WriteLine($"Capitalize: '{str}' -> '{res}'");
            return res;
        }

        /*
         * \s - space-symbols
         * \S - non-space
         * \d - digit
         * \D - non-digit
         * \w - word-symbol
         * \W - non-word
         * \b - boundary
         * ^ - почаок рядка
         * $ - кінець рядка
         * . - довільний символ
         * 
         * [0-9a-fA-F] - група символів (один з набору)
         * [^0-9] - група вийнятків (усе окрім)
         * 
         * ? - один або жодного
         * + - один і більше
         * * - довільна кількість (у т.ч. відсутність)
         * {2} - рівно 2
         * {2,5} - від 2 до 5
         * {2,} - більше 2
         */
        private String Spacefy(String str)
        {
            Task.Delay(1000).Wait();
            String res = Regex.Replace(str, @"\s+", " ");
            Console.WriteLine($"Spacefy: '{str}' -> '{res}'");
            return res;
        }

        private String GetString()
        {
            Task.Delay(1000).Wait();
            String str = "!-!;The quick - \tbrown \t fox         jumps over,; the lazy; dog?;'?";
            Console.WriteLine($"GetString: '{str}' ");
            return str;
        }

        public void RunWrong()
        {
            startTicks = DateTime.Now.Ticks;
            Task<String> task1 = Task.Run(Work1);
            Task<String> task2 = Task.Run(Work2);

            Console.WriteLine("{1:F2} Work1 res: {0}", task1.Result, ((DateTime.Now.Ticks - startTicks) % (long)1e8) / 1e7);
            Task<String> task3 = Task.Run(() => Work3(task1.Result));

            Console.WriteLine("{1:F2} Work2 res: {0}", task2.Result, ((DateTime.Now.Ticks - startTicks) % (long)1e8) / 1e7);
            Task<String> task4 = Task.Run(() => Work4(task2.Result));

            Console.WriteLine("{1:F2} Work3 res: {0}", task3.Result, ((DateTime.Now.Ticks - startTicks) % (long)1e8) / 1e7);
            Console.WriteLine("{1:F2} Work4 res: {0}", task4.Result, ((DateTime.Now.Ticks - startTicks) % (long)1e8) / 1e7);
            
            Console.WriteLine("{0:F2} Demo finish", ((DateTime.Now.Ticks - startTicks) % (long)1e8) / 1e7);
        }

        private String Work1() => Worker(1, 2000);
        private String Work2() => Worker(2, 1000);
        private String Work3(String data = "") => Worker(3, 1000, data);
        private String Work4(String data = "") => Worker(4, 2000, data);

        private String Worker(int num, int delay, String data="")
        {
            Console.WriteLine("{0:F2} Work{1} start {2}", ((DateTime.Now.Ticks - startTicks) % (long)1e8) / 1e7, num, data);
            Task.Delay(delay).Wait();
            Console.WriteLine("{0:F2} Work{1} finish", ((DateTime.Now.Ticks - startTicks) % (long)1e8) / 1e7, num);
            return $"Work{num} result";
        }
    }
}
