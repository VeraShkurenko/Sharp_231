using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.IO;

namespace Sharp_231.Dict
{
    internal class DictDemo
    {
        private const string filename = "dictionary.json";
        private Dictionary<string, string> dictionary;

        class MenuItem
        {
            public string Title { get; set; } = null!;
            public char Key { get; set; }
            public Action Action { get; set; } = null!;
            public override string ToString() => $"{Key}. {Title}";
        }

        private MenuItem[] menuItems;

        public DictDemo()
        {
            try
            {
                dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(
                    File.ReadAllText(filename))!;
                Console.Write("Словник завантажено: ");
            }
            catch
            {
                dictionary = new Dictionary<string, string>()
                {
                    { "apple", "яблуко" },
                    { "pear", "груша" },
                    { "plum", "слива" },
                    { "peach", "персик" },
                    { "grape", "виноград" }
                };
                Console.Write("Словник створено: ");
            }
            Console.WriteLine(dictionary.Count + " слів");

            menuItems = new MenuItem[]
            {
                new MenuItem(){Key ='0',Title = "Вихід з програми", Action = ()=>throw new Exception() },
                new MenuItem(){Key ='1', Title = "Переклад слова з української до англійської", Action = Uk2En },
                new MenuItem(){Key = '2', Title = "Переклад слова з англійської до української", Action = En2Uk },
                new MenuItem(){Key = '3', Title = "Додати слово до словника", Action = AddWord },
                new MenuItem(){Key = '4', Title = "Вивести весь словник", Action = PrintDict },
                new MenuItem(){Key = '5', Title = "Редагувати існуюче слово за англійським словом", Action = EditWord },
                new MenuItem(){Key = '6', Title = "Редагувати існуюче слово за українським словом", Action = EditWordByUkrainian },
                new MenuItem(){Key = '7', Title = "Видалити слово за англійським словом", Action = DeleteByEnglish },
                new MenuItem(){Key = '8', Title = "Видалити слово за українським словом", Action = DeleteByUkrainian }
            };
        }

        private void SaveDictionary()
        {
            File.WriteAllText(filename,
                JsonSerializer.Serialize(dictionary,
                    new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true
                    }));
        }

        public void Run()
        {
            try
            {
                while (true)
                    Menu();
            }
            catch { }
            finally
            {
                SaveDictionary();
            }
        }

        public void Menu()
        {
            ConsoleKeyInfo key;
            MenuItem? selectedItem;
            do
            {
                Console.WriteLine("\nСловник:");
                foreach (MenuItem item in menuItems)
                    Console.WriteLine(item);

                key = Console.ReadKey();
                selectedItem = menuItems.FirstOrDefault(item => item.Key == key.KeyChar);
                Console.WriteLine();

                if (selectedItem == null)
                    Console.WriteLine(" - Невірний вибір, спробуйте ще раз.");
                else
                    selectedItem.Action();

            } while (selectedItem == null);
        }

        private void AddWord()
        {
            Console.Write("Введіть слово англійською: ");
            string en = Console.ReadLine()!.Trim();
            Console.Write("Введіть слово українською: ");
            string uk = Console.ReadLine()!.Trim();
            try
            {
                dictionary.Add(en, uk);
                Console.WriteLine("Слово додано!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Виникла помилка: " + ex.Message);
            }
        }

        private void Uk2En()
        {
            Console.Write("Введіть слово українською: ");
            string? word;
            do
            {
                word = Console.ReadLine()?.Trim();
            } while (string.IsNullOrEmpty(word));

            var tr = dictionary.Where(pair => pair.Value == word);
            if (tr.Any())
            {
                foreach (var pair in tr)
                    Console.WriteLine("{0} -- {1}", word, pair.Key);
            }
            else
            {
                Console.WriteLine("Слова '{0}' немає у словнику.", word);
                SuggestClosest(word);
            }
        }

        private void En2Uk()
        {
            Console.Write("Введіть слово англійською: ");
            string? word;
            do
            {
                word = Console.ReadLine()?.Trim();
            } while (string.IsNullOrEmpty(word));

            if (dictionary.ContainsKey(word))
                Console.WriteLine("{0} -- {1}", word, dictionary[word]);
            else
            {
                Console.WriteLine("Слова '{0}' немає у словнику.", word);
                SuggestClosest(word);
            }
        }

        private void PrintDict()
        {
            const int pageSize = 5;
            int total = dictionary.Count;
            int index = 0;

            while (index < total)
            {
                Console.Clear();
                Console.WriteLine("Використовуйте → для наступної сторінки, ← для попередньої, ESC для виходу.\n");

                foreach (var pair in dictionary.Skip(index).Take(pageSize))
                    Console.WriteLine("{0} -- {1}", pair.Key, pair.Value);

                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.RightArrow && index + pageSize < total)
                    index += pageSize;
                else if (key == ConsoleKey.LeftArrow && index - pageSize >= 0)
                    index -= pageSize;
                else if (key == ConsoleKey.Escape)
                    break;
            }
        }

        private void EditWord()
        {
            Console.Write("Введіть англійське слово для зміни: ");
            string? word;
            do
            {
                word = Console.ReadLine()?.Trim();
            } while (string.IsNullOrEmpty(word));

            if (!dictionary.ContainsKey(word))
            {
                Console.WriteLine("Слова '{0}' немає у словнику.", word);
                return;
            }

            Console.WriteLine("Поточний переклад: {0}", dictionary[word]);
            Console.Write("Введіть новий переклад: ");
            string? newWord;
            do
            {
                newWord = Console.ReadLine()?.Trim();
            } while (string.IsNullOrEmpty(newWord));

            dictionary[word] = newWord;
            Console.WriteLine("Запис оновлено: {0} -- {1}", word, newWord);
        }

        private void EditWordByUkrainian()
        {
            Console.Write("Введіть українське слово для зміни: ");
            string? ukWord;
            do
            {
                ukWord = Console.ReadLine()?.Trim();
            } while (string.IsNullOrEmpty(ukWord));

            var matches = dictionary.Where(p => p.Value == ukWord).ToList();

            if (!matches.Any())
            {
                Console.WriteLine("Слова '{0}' немає у словнику.", ukWord);
                return;
            }

            foreach (var pair in matches)
            {
                Console.WriteLine("Поточний запис: {0} -- {1}", pair.Key, pair.Value);
                Console.Write("Введіть новий переклад: ");
                string? newTranslation = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(newTranslation))
                {
                    dictionary[pair.Key] = newTranslation;
                    Console.WriteLine("Запис оновлено: {0} -- {1}", pair.Key, newTranslation);
                }
            }
        }

        private void DeleteByEnglish()
        {
            Console.Write("Введіть слово для видалення англійською: ");
            string? word;
            do
            {
                word = Console.ReadLine()?.Trim();
            } while (string.IsNullOrEmpty(word));

            if (!dictionary.ContainsKey(word))
            {
                Console.WriteLine("Слова '{0}' немає у словнику.", word);
                return;
            }

            Console.Write("Ви впевнені, що хочете видалити слово? (y/n): ");
            var confirm = Console.ReadKey().KeyChar;
            Console.WriteLine();
            if (confirm == 'y' || confirm == 'Y')
            {
                dictionary.Remove(word);
                Console.WriteLine("Слово '{0}' успішно видалено!", word);
            }
            else
            {
                Console.WriteLine("Видалення скасовано.");
            }
        }

        private void DeleteByUkrainian()
        {
            Console.Write("Введіть слово для видалення українською: ");
            string? word;
            do
            {
                word = Console.ReadLine()?.Trim();
            } while (string.IsNullOrEmpty(word));

            var matches = dictionary.Where(pair => pair.Value == word).ToList();

            if (!matches.Any())
            {
                Console.WriteLine("Слова '{0}' немає у словнику.", word);
                return;
            }

            Console.Write("Ви впевнені, що хочете видалити всі записи? (y/n): ");
            var confirm = Console.ReadKey().KeyChar;
            Console.WriteLine();
            if (confirm == 'y' || confirm == 'Y')
            {
                foreach (var pair in matches)
                {
                    dictionary.Remove(pair.Key);
                    Console.WriteLine("Видалено: {0} -- {1}", pair.Key, pair.Value);
                }
            }
            else
            {
                Console.WriteLine("Видалення скасовано.");
            }
        }

        private void SuggestClosest(string word)
        {
            Console.WriteLine("Можливо, ви мали на увазі:");
            var closest = dictionary
                .Select(pair => new { En = pair.Key, Uk = pair.Value, LD = LevenshteinDistance(word, pair.Key) })
                .Where(x => x.LD <= 2)
                .OrderBy(x => x.LD)
                .Take(5);

            foreach (var item in closest)
                Console.WriteLine(" - {0} -- {1} (відстань {2})", item.En, item.Uk, item.LD);
        }

        private int LevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; i++) d[i, 0] = i;
            for (int j = 0; j <= m; j++) d[0, j] = j;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
        }
    }
}
