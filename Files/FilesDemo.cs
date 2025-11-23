using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Sharp_231.Files
{
    internal class FilesDemo
    {
        int EndingType(int number)
        {
            number = number % 100;
            if (number >= 5 && number <= 20) return 0;
            number = number % 10;
            if (number == 0 || number >= 5) return 0;
            if (number == 1) return 1;
            return 2;
        }

        public void Run()
        {
            //Serializing
            Library.Library library = new();
            library.Init();
            var options = new JsonSerializerOptions { 
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true 
            };
            String lib = System.Text.Json.JsonSerializer.Serialize(library, options);
            Console.WriteLine(lib);
            File.WriteAllText("library.json", lib);
            
            //Deserializing
            Library.Library library2;
            try
            {
                library2 = JsonSerializer.Deserialize<Library.Library>(lib)!;
                                    // ?? throw new NullReferenceException();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            library2.PrintCatalog();

            Vectors.Vector v = new() { X = 10, Y = 20 };
            string j = System.Text.Json.JsonSerializer.Serialize(v);
            Console.WriteLine(j);
            Vectors.Vector v2 = System.Text.Json.JsonSerializer.Deserialize<Vectors.Vector>(j);
            Console.WriteLine(v2);
        }
        public void RunLog()
        {
            string logDir = Directory.GetCurrentDirectory() + "/logs";
            if (!Directory.Exists(logDir))
            {
                try
                {
                    Directory.CreateDirectory(logDir);
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Неможливо створити директорію логування " + ex.Message);
                    return;
                }
            }

            String logFile = "runlogs.txt";
            String logPath = Path.Combine(logDir, logFile);
            if (!File.Exists(logPath))
            {
                try
                {
                    File.Create(logPath).Dispose();
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Неможливо створити файл логування " + ex.Message);
                    return;
                }

            }
           
            FileInfo f = new FileInfo(logPath);
            if (f.Length > 100)
            {
                int i = 1;
                string newLogName;
                do
                {
                    newLogName = $"runlogs_{i}.txt";
                    i++;
                } while (File.Exists(Path.Combine(logDir, newLogName)));
                logPath = Path.Combine(logDir, newLogName);
                File.Create(logPath).Dispose();
            }
            try
            {
                File.AppendAllText(logPath, DateTime.Now.ToString() + "\n");

            }
            catch (IOException ex)
            {
                Console.WriteLine("Помилка логування " + ex.Message);
                return;
            }

            string[] lines = File.ReadAllLines(logPath);
            string times = EndingType(lines.Length) switch
            {
                0 => "разів",
                1 => "раз",
                2 => "рази",
                _ => throw new ArgumentException("Unexpected ending type")
            };
            Console.WriteLine($"Програма запускалась {lines.Length} " + times + ":");

            for (int i = 0; i < lines.Length; i++)
            {
                Console.Write($"{i + 1}. ");
                Console.WriteLine(lines[i]);
            }
        }

        public void Run4()
        {
            string dir = Directory.GetCurrentDirectory();
            string filepath = Path.Combine(dir, "file4.txt");
            try
            {
                File.WriteAllText(filepath, "File 4 Content");
                File.AppendAllText(filepath, "\nAppend line");
                Console.WriteLine(File.ReadAllText(filepath));
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void Run3()
        {
            try
            {
                using var wtr = new StreamWriter("./file3.txt");
                wtr.Write("x=3");
                wtr.Write(10);

            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            try
            {
                using var rdr = new StreamReader("./file3.txt");
                string content = rdr.ReadToEnd();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void Run2()
        {

            Console.WriteLine("Files Demo");
            FileStream? fs = null;
            try
            {
                fs = new FileStream("./file.txt", FileMode.Create, FileAccess.Write);
                fs.Write(Encoding.UTF8.GetBytes("Hello world"));
                fs.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                fs?.Close();
            }
            try
            {
                using var rfs = new FileStream("./Files/file2.txt", FileMode.Open, FileAccess.Read);
                byte[] buff = new byte[4096];
                int n = rfs.Read(buff, 0, 4096);
                Console.WriteLine(Encoding.UTF8.GetString(buff, 0, n));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
/*
  Робота хз файлами в основі має потоки(stream)
Потоки дозволяють маніпулювати  з одгим байтом, у т.ч 
лдпзу мати спробу з масимовом байтів. 
для запису/читання інших типів даних необхідно вживати заходи 
з їхперетворення до бінарного коду

Про перетворенні числа:
- пряме представлення (32 біти) 
- рядкове приставлення  "2342"

Потоки (!файлові) є некерованими ресурсами і вимагають
закриття подачею команди
(якщо не зкарити, то платформа цього зробити не зможе)
using - авто disposable - блок з автоматичним закриття 

Пряма робота з потоками незручна при збереженні різних
типів даних. Тому вживаються "обгортки" StreamReader / StreamWriter

Серіалізація (послідовний) - спосіб представити об'єкт у вигляді послідовності,
що може бути збережена чи передана послідовним каналом
є різні форми серіалізації: бінарна та текстова
Серед текстових найбільш поширена - json 


 */

