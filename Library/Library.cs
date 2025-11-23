using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sharp_231.Library
{
    public class Library
    {
        [JsonInclude]
        private List<Literature> Funds { get; set; } = [];

        public static Library FromJson(String json)
        {
            Library library = new Library();
            library.Funds.Clear();

            var j = JsonSerializer.Deserialize<JsonNode>(json);
            foreach (var element in j.AsObject()["Funds"].AsArray())
            {
                JsonObject jo = element.AsObject();
                if (jo.ContainsKey("Author"))
                {
                    library.Funds.Add(jo.GetValue<Book>());
                }
            }
            return library;
        }

        public Library()
        {

        }
        public void Init()
        {
            Funds.Add(new Book
            {
                Author = "D. Knuth",
                Title = "The art of programming",
                Publisher = "Київ, Наукова Думка",
                Year = 2000
            });

            Funds.Add(new Book
            {
                Author = "J. Richterr",
                Title = "CLR via C#",
                Publisher = "Microsoft Press",
                Year = 2013
            });

            Funds.Add(new Journal
            {
                Title = "ArgC & ArgV",
                Number = "2(113), 2000",
                Publisher = "https://journals.ua/technology/argc-argv/"
            });

            Funds.Add(new Newspaper
            {
                Title = "Урядовий кур'єр",
                Date = new DateOnly(2025, 10, 23),//які діє оператор NEW на структури
                                                  // оскільки структура — ValueType, то значення замінюється в області
                                                  // змінної шляхом перезапуску конструктора структури
                Publisher = "Газета Кабінету Міністрів України"
            });

            Funds.Add(new Booklet
            {
                Title = "New Opening!",
                Subject = "Sweet Restaurant",
                Author = "PopShop"
            });

            Funds.Add(new Hologram
            {
                Title = "Скіфське мистецтво",
                ArtItem = "Золота пектораль",
                Publisher = "Студія 'Лазер'"
            });

            Funds.Add(new Poster
            {
                Title = "Star Wars",
                Subject = "A New Hope",
                Publisher = "20th Century Fox"
            });
        }
        public void ShowCiteCard(string StyleName)
        {
            foreach (Literature literature in Funds)
            {
                foreach (var method in literature.GetType().GetMethods())
                {
                    var attr = method.GetCustomAttribute<CiteStyleAttribute>();
                    if (attr != null && attr.Style==StyleName)
                    {
                        method.Invoke(literature, null);
                    }
                }
            }

        }
        public void ShowColorPrintable()
        {
            //пошук за атрибутами мета данними, що супроводжують методи
            foreach (Literature literature in Funds)
            {
                foreach(var method in literature.GetType().GetMethods())
                {
                    var attr = method.GetCustomAttribute<ColorPrintAttribute>();
                    if(attr !=null)
                    {
                        for (int i =0;i<attr.Copies; i++)
                        {
                            method.Invoke(literature, ["RGB"]);
                        }
                    }
                }
            }
        }
        public void ShowPrintable()
        {
            // Duck Typing - find objects of any type with Print() method
            foreach (Literature literature in Funds)
            {
                MethodInfo? printMethod = literature.GetType().GetMethod("Print");
                if (printMethod != null)
                {
                    //Method invocation
                    //              object    args - values for params
                    printMethod.Invoke(literature, null);
                }
            }
        }

        public void PrintCatalog()
        {
            foreach (Literature literature in Funds)
            {
                Console.WriteLine(literature.GetCard());
            }
        }

        public void PrintPeriodic()
        {
            foreach (Literature literature in Funds)
            {
                if (literature is IPeriodic lit)
                {
                    // Прямо через literature метод GetPeriod не доступний
                    // (хоча ми певні, що він там є, бо умова "is IPeriodic" виконана)
                    // Для доступу до методу інтерфейсу необхідно перетворити
                    // тип змінної до інтерфейсного
                    // literature as IPeriodic - референсне(м'яке) перетворення,
                    // може дати null, якщо типи не сумісні
                    //(IPeriodic)literature - "жорстке" перетворення, кидається вийнятком
                    // Pattern matching - if (literature is IPeriodic lit) більш сучасний інструмент 
                    Console.WriteLine("Раз у " + lit.GetPeriod() + ": ");
                    Console.WriteLine(literature.GetCard());
                }

            }
        }

        public void PrintNonPeriodic()
        {
            foreach (Literature literature in Funds)
            {
                if (literature is not IPeriodic)
                {
                    Console.WriteLine(literature.GetCard());
                }
            }
        }

        public void ShowNonPrintable()
        {
            foreach (Literature literature in Funds)
            {
                if (literature is INonPrintable)
                {
                    Console.WriteLine(literature.GetCard());
                }
            }
        }
    }
}

/*
[] - оперативна пам'ять
() - програма(збірка)
{} - постійна пам'ять (диск, bios)

    запуск                      Value Type                    Reference Type
{(program), ....}             {(32b), ....}                  {(*64bit), ....}
     |                              
     v                            x = 10                         x = new()
[(program), ....]             [(10), ....]`               [(ref), ....(obj#ref)]
                             null - invalid                   \__________/
                              x = new(20)?                      x = new(20)?
                          перезапуск конструктора         утворення новоого об'єкту 
                         без створення додаткових             і заміна посилання
                                 об'єктів       
                              [(20), ....]               [(ref), ....(obj#ref)...(20#ref)]
                                                            \____xx____/              /
                                                             \_______________________/

A=B                копія значення В до А              утворення другого посилання на В

В.х = 10           змінюється тільки В.х             змінюється поле "х" єдиного об'єкту, 
                                                            тобто як В.х, так і А.х
                                
 */