using Sharp_231.Networking.Api;
using Sharp_231.Networking.Orm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sharp_231.Networking
{
    internal class NetworkingDemo
    {
        public async Task Run()
        {
            using HttpClient client = new();
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange"),
            };
            Task<HttpResponseMessage> responseTask =
                client.SendAsync(request);

            Console.WriteLine("Курси валют НБУ, робота з XML");
            Console.Write(DateTime.Now.Ticks / 10000 % 100000); Console.WriteLine(" request start");

            HttpResponseMessage response = await responseTask;
            Task<String> contentTask = response.Content.ReadAsStringAsync();

            Console.Write(DateTime.Now.Ticks / 10000 % 100000); Console.WriteLine(" request finish");
            String xmlString = await contentTask;
            XDocument xmlDocument = XDocument.Parse(xmlString);

            List<NbuRate> rates = [..
                xmlDocument
                .Descendants("currency")
                .Select(c => new NbuRate
                {
                    Txt = c.Element("txt")!.Value,
                    Rate = Convert.ToDouble(c.Element("rate")!.Value, CultureInfo.InvariantCulture),
                    Cc = c.Element("cc")!.Value,
                    R030 = Convert.ToInt32(c.Element("r030")!.Value),
                    Exchangedate = DateOnly.Parse(c.Element("exchangedate")!.Value),
                    Special = c.Element("special")!.Value
                })
            ];
            Console.WriteLine(String.Join('\n', rates));
        }

        public async Task RunXml()
        {
            Console.WriteLine("Курси валют НБУ, робота з XML");
            Console.Write("Введіть дату (дд.мм.рррр): ");
            DateTime date = DateTime.Parse(Console.ReadLine()!);
            if (date > DateTime.Now)
            {
                Console.WriteLine("Цей день ще не настав...");
                return;
            }
            String formattedDate = $"{date:yyyy}{date:MM}{date:dd}";

            using HttpClient client = new();
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?date={formattedDate}"),
            };
            Task<HttpResponseMessage> responseTask =
                client.SendAsync(request);

            Console.Write(DateTime.Now.Ticks / 10000 % 100000); Console.WriteLine(" request start");

            HttpResponseMessage response = await responseTask;
            Task<String> contentTask = response.Content.ReadAsStringAsync();

            Console.Write(DateTime.Now.Ticks / 10000 % 100000); Console.WriteLine(" request finish");
            String xmlString = await contentTask;
            XDocument xmlDocument = XDocument.Parse(xmlString);

            List<NbuRate> rates = [..
                xmlDocument
                .Descendants("currency")
                .Select(c => new NbuRate
                {
                    Txt = c.Element("txt")!.Value,
                    Rate = Convert.ToDouble(c.Element("rate")!.Value, CultureInfo.InvariantCulture),
                    Cc = c.Element("cc")!.Value,
                    R030 = Convert.ToInt32(c.Element("r030")!.Value),
                    Exchangedate = DateOnly.Parse(c.Element("exchangedate")!.Value),
                    Special = c.Element("special")!.Value
                })
            ];
            Console.WriteLine(String.Join('\n', rates));
        }

        public async Task RunJson()
        {
            Console.WriteLine("Курси валют НБУ, робота з JSON");
            Console.Write("Введіть дату (дд.мм.рррр): ");
            DateTime date = DateTime.Parse(Console.ReadLine()!);
            if (date > DateTime.Now)
            {
                Console.WriteLine("Цей день ще не настав...");
                return;
            }
            String formattedDate = $"{date:yyyy}{date:MM}{date:dd}";

            HttpClient client = new();
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?date={formattedDate}&json")
            };
            Task<HttpResponseMessage> responseTask = client.SendAsync(request);


            Console.Write(DateTime.Now.Ticks / 10000 % 100000); Console.WriteLine(" request start");

            HttpResponseMessage response = await responseTask;
            Task<String> contentTask = response.Content.ReadAsStringAsync();

            Console.Write(DateTime.Now.Ticks / 10000 % 100000); Console.WriteLine(" request finish");
            String jsonString = await contentTask;
            List<NbuRate> rates = NbuApi.ListFromJsonString(jsonString);

            foreach (NbuRate rate in rates)
            {
                Console.WriteLine(rate);
            }
        }

        public async Task Run3()
        {
            using HttpClient client = new();
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json")
            };
            Task<HttpResponseMessage> responseTask = client.SendAsync(request);

            Console.WriteLine("HTTP requests and responses");
            Console.Write(DateTime.Now.Ticks / 10000 % 100000); Console.WriteLine(" request start");

            HttpResponseMessage response = await responseTask;
            Task<String> contentTask = response.Content.ReadAsStringAsync();

            Console.Write(DateTime.Now.Ticks / 10000 % 100000); Console.WriteLine(" request finish");
        }

        public async Task RunStep()
        {
            using HttpClient client = new();
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://itstep.org/")
            };
            Task<HttpResponseMessage> responseTask = client.SendAsync(request);
            
            Console.WriteLine("HTTP requests and responses");
            Console.Write(DateTime.Now.Ticks / 10000 % 100000); Console.WriteLine(" request start");

            HttpResponseMessage response = await responseTask;
            Task<String> contentTask = response.Content.ReadAsStringAsync();
            
            Console.Write(DateTime.Now.Ticks / 10000 % 100000); Console.WriteLine(" request finish");

            Console.WriteLine($"HTTP/{response.Version} {(int)response.StatusCode} {response.ReasonPhrase}");
            foreach (var header in response.Headers)
            {
                Console.WriteLine("{0}: {1}", header.Key, String.Join(',', header.Value));
            }
            Console.WriteLine();
            Console.WriteLine(await contentTask);
        }

        public void RunSite()
        {
            HttpClient client = new();
            Console.Write("Insert Site URL: ");
            string url = Console.ReadLine()!;
            Task<String> getRequest = client.GetStringAsync(url.Contains("://") ? url : "https://" + url);
            try
            {
                var p = Process.Start(new ProcessStartInfo
                {
                    FileName = "\"C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe\"",
                    Arguments = url
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Виникла помилка: {ex.Message}");
            }
            Console.Write(DateTime.Now.Ticks / 10000 % 100000);
            Console.WriteLine(" Get start");
            String requestBody = getRequest.Result;
            Console.WriteLine(DateTime.Now.Ticks / 10000 % 100000);
            Console.WriteLine(requestBody);
        }

        public void RunBody()
        {
            HttpClient client = new();
            Task<String> getRequest = client.GetStringAsync("https://www.tavriav.ua/search?name=%D1%8F%D0%B9%D1%86%D1%8F");
            Console.Write(DateTime.Now.Ticks / 10000 % 100000);
            Console.WriteLine(" Get start");
            String requestBody = getRequest.Result;
            Console.WriteLine(DateTime.Now.Ticks / 10000 % 100000);
            Console.WriteLine(requestBody);
        }
    }
}

/*
 * Мережа - сукупність вузлів та зв'язків між ними (каналів зв'язку)
 * Вузол (Node) - активний учасник, що перетворює дані (ПК, принтер, телефон тощо)
 *    вузол у мережі відрізняється адресою та/або іменем
 * Зв'зяок - спосіб передачі даних між вузлами (дріт, оптоволокно, радіоканал тощо)
 * 
 * HTTP - текстовий транспортний протокол
 * запит                відповідь
 * метод шлях           статус-код   фраза
 * заголовки - пари     ключ: значення\r\n
 * тіло (довільна інформація), зокрема, JSON - текстовий протокол передачі даних
 * 
 * 
 * CONNECT  службові
 * TRACE
 * 
 * HEAD     технологічні
 * OPTIONS
 * 
 *          загальні CRUD - Create Read Update Delete
 * GET      одержання даних (читання, Read) -- без модифікації системи (без змін)
 * POST     створення нових елементів (Create)
 * DELETE   
 * PUT      заміна наявних даних на передані
 * PATCH    оновлення частини наявних даних
 * 
 *          галузеві стандарти
 * LINK
 * UNLINK
 * PURGE
 * MKCOL
 * 
 * біт - одиниця вимірювання інформаційної ентропії
 */
