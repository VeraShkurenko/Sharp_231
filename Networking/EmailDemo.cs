namespace SharpKnP321.Networking
{
    internal class EmailDemo
    {
        public void Run()
        {
            Console.WriteLine("--- Демонстрація OTP + Email Service ---");

            try
            {
                // 1. Генеруємо пароль (наприклад, 6 символів, змішаний режим)
                string password = OtpService.Generate(6, OtpMode.Mixed);
                Console.WriteLine($"Згенеровано OTP: {password}");

                // 2. "Надсилаємо" його через наш імітаційний сервіс
                EmailService.Send(
                    to: "student@example.com",
                    subject: "Ваш код підтвердження",
                    body: $"<h1>Ваш одноразовий пароль: {password}</h1>",
                    isHtml: true
                );

                Console.WriteLine("Процес завершено успішно.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
        public void Run1()
        {
            Console.WriteLine("Демонстрація роботи EmailService");

            try
            {
                // Використовуємо наш новий метод
                EmailService.Send(
                    to: "azure.spd111.od.0@ukr.net",
                    subject: "ДЗ: Перевірка сервісу",
                    body: "<h1>Успішно!</h1> <p>Метод працює, конфігурація кешується.</p>",
                    isHtml: true
                );

                Console.WriteLine("Лист успішно відправлено!");
            }
            catch (Exception ex)
            {
                // Обробка помилок, як і просив викладач
                Console.WriteLine($"Критична помилка: {ex.Message}");
            }
        }
    }
}