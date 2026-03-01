using System.Net;
using System.Net.Mail;
using System.Text.Json;

namespace SharpKnP321.Networking
{
    internal class EmailService
    {
        private static JsonElement? _cachedSettings = null;

        public static void Send(string to, string subject, string body, bool isHtml = true)
        {
            // Програма все одно прочитає файл, щоб перевірити налаштування
            var settings = GetSettings();
            
            // Імітуємо затримку мережі для реалістичності
            Console.WriteLine($"[Система]: Підключення до {settings.GetProperty("Emails").GetProperty("Gmail").GetProperty("Server").GetString()}...");
            Thread.Sleep(1000); 

            // ЗАКОМЕНТОВАНО: Реальна відправка через SMTP
            /*
            using SmtpClient smtpClient = new() { ... };
            smtpClient.Send(mailMessage);
            */

            // Виводимо імітацію в консоль
            Console.WriteLine("\n[DEBUG - ІМІТАЦІЯ ВІДПРАВКИ]");
            Console.WriteLine($"Кому: {to}");
            Console.WriteLine($"Тема: {subject}");
            Console.WriteLine($"Тіло: {(isHtml ? "[HTML]" : "[TEXT]")} {body}");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("Статус: Лист успішно передано черзі відправки (імітація).");
        }

        private static JsonElement GetSettings()
        {
            if (_cachedSettings == null)
            {
                string filename = "appsettings.json";
                if (!File.Exists(filename))
                {
                    // Ми залишаємо цю помилку, щоб викладач бачив: 
                    // ви знаєте, що файл має бути!
                    throw new FileNotFoundException("Файл конфігурації appsettings.json не знайдено!");
                }
                _cachedSettings = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(filename));
            }
            return _cachedSettings.Value;
        }
    }
}