namespace SharpKnP321.Networking
{
    internal class EmailDemo
    {
        public void Run()
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