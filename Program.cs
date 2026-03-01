//Колекції: можуть мати об'єкти різного типу, не обов'язково послідовні, варіативний розмір
using Sharp_231.Collection;
using Sharp_231.Data;
using Sharp_231.Dict;
using Sharp_231.Events;
using Sharp_231.Exeptions;
using Sharp_231.Extensions;
using Sharp_231.Files;
using Sharp_231.Fractions;
using Sharp_231.Library;
using Sharp_231.Vectors;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Sharp_231.AsyncProgramming;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

//ShowLibrary();
//ShowReflection();
//new VectorDemo().Run();
//new FractionDemo().Run();

try
{
    //Console.OutputEncoding = System.Text.Encoding.Unicode;
    //Console.InputEncoding = System.Text.Encoding.Unicode;
    // new DictDemo().Run();
    // new DataDemo().Run();
    //new AsyncProgramming().Run();
    // new ThreadJoin().Run();
    // new AsyncDemo().Run();
    // await new AsyncDemo().Run();
    new Continuations().Run();
}
catch (Exception ex)
{
    //логічних дій з винятком на данному рівні передбачити важко
    //здійснюється логування(запис даних) про аварійну зупинку програми
    //у режимі розробника це ще може бути error-page
     Console.WriteLine("Не оброблений у програмі виняток: " + ex.ToString());
}
void ShowReflection()
{
    /*
     Рефлекксія в ООП - інструментарій мови/платформи, 
     який дозволяє одержувати відомості про склад типу даних
     */
    Type bookType = typeof(Book);
    FieldInfo[] fields = bookType.GetFields();
    if (fields.Length > 0)
    {
        Console.WriteLine("Type 'Book' has fields: ");
        foreach (var field in fields)
        {
            Console.WriteLine(field.Name);
        }
    }
    else
    {
        Console.WriteLine("Type 'Book' has NO fields!");
    }
    PropertyInfo[] props = bookType.GetProperties();
    if (props.Length > 0)
    {
        Console.WriteLine("Type 'Book' has props: ");
        foreach (var prop in props)
        {
            Console.WriteLine("{0}:{1}", prop.Name, prop.PropertyType.Name);
        }
    }
    else
    {
        Console.WriteLine("Type 'Book' has NO props!");
    }
    MethodInfo[] meths = bookType.GetMethods();
    if (meths.Length > 0)
    {
        Console.WriteLine("Type 'Book' has methods: ");
        foreach (var meth in meths)
        {
            Console.WriteLine(meth.Name);
        }
    }
    else
    {
        Console.WriteLine("Type 'Book' has NO methods!");
    }
    EventInfo[] events = bookType.GetEvents();
    if (events.Length > 0)
    {
        Console.WriteLine("Type 'Book' has events: ");
        foreach (var eve in events)
        {
            Console.WriteLine(eve.Name);
        }
    }
    else
    {
        Console.WriteLine("Type 'Book' has NO events!");
    }
    Console.WriteLine("\n-------------------Рефлексія за об'єктом------------------");
    Literature j = new Journal()
    {
        Title = "ArgC & ArgV",
        Number = "2(113), 2000",
        Publisher = "https://journals.ua/technology/argc-argv/"
    };
    Type jType = j.GetType();
    Console.WriteLine(jType.Name);
    // Journal - змінна типізується за об'єктом, а не за оголошенням
    PropertyInfo? propN = jType.GetProperty("Number");
    if (propN != null)
    {
        //prop - відомості про тип даних, а не про об'єкт
        var number = propN.GetValue(j);// -> j.Number
        Console.WriteLine($"Object has 'Number' property with value '{number}'");
    }
    else
    {
        //"класична типізація" - якщо щось ходить як качка
        //та видає звуки як качка, то це і є качка
        //програмний прийом за якого визначається
        //не сам тип, а наявність у ньому певних методів/складових
        //замість перевірки умовного інтерфейсму IPrintable
        //перевіряється наявність метода Print()

        Console.WriteLine($"Object has no 'Number'");
    }
    Library library = new();
    Console.WriteLine("\n---------------Printable---------------");
    library.ShowPrintable();

    Console.WriteLine("\n-------------Color Printable-----------");
    library.ShowColorPrintable();

    Console.WriteLine("\n-------------Non Printable-------------");
    library.ShowNonPrintable();


    Console.WriteLine("\n-------------------APA-----------------");
    library.ShowCiteCard("APA");
    Console.WriteLine("\n------------------IEEE-----------------");
    library.ShowCiteCard("IEEE");

}
void ShowLibrary()
{
    Library library = new();
    library.PrintCatalog();
    Console.WriteLine("\n__________Periodic______________");
    library.PrintPeriodic();
    Console.WriteLine("\n_________Non_Periodic___________");
    library.PrintNonPeriodic();
    Console.WriteLine("________________________________");
}

void Collections()
{
    //\ед ліст більш корисний для модифікаці, встановлуються, видаляються елеиенти
    List<string> list = new List<string> { "str1", "str2", "str3" };
    list.Add("str1");
    list.Add("str2");
    list.Add("str3");

    list.RemoveAt(0);
    foreach (string s in list)
    {
        Console.WriteLine(s);
    };
    LinkedList<string> list2 = new LinkedList<string>();
    list2.AddLast("str1");
    list2.AddLast("str2");
    list2.AddLast("str3");


    foreach (string s in list)
    {
        Console.WriteLine(s);
    }
    //Асоціативні масиви, словники 
    Dictionary<string, string> map = new Dictionary<string, string>();
    map.Add("key 1", "valiue 1");

    map["key 2"] = "value 2";
    foreach (var pair in map)
    {
        Console.WriteLine("{0} {1}", pair.Key, pair.Value);
    }


    //String - immutable
    string str = "";
    foreach (string s in list) str += s;


    StringBuilder sb = new();
    foreach (string s in list)
    {
        sb.Append(s);
    }
    Console.WriteLine(sb.ToString());


    //Масиви:однотипні, послідовні, фісований розмір
    int[] arr = new int[10];
    foreach (int el in arr)
    {
        Console.WriteLine(el);

    }
    Console.WriteLine(arr[1]);
    for (int i = 0; i < arr.Length; i++)
    {
        Console.WriteLine(arr[i]);
    }

    arr[0] = default;
    int[][] arr2 = new int[5][];
    for (int i = 0; i < 5; i++)
    {
        arr2[i] = new int[i + 1];
    }
    foreach (var el in arr2)
    {
        foreach (var w in el)
        {
            Console.Write(w + " ");
        }
        Console.WriteLine();
    }
    int[,] arr3 = new int[3, 4];
    for (int i = 0; i < 3; i++)
    {
        for (int j = 0; j < 4; j++)
        {
            Console.Write(arr3[i, j]);
        }
    }
    string[] strings = { "s1", "s2" };
    String[] s_2 = { "s1, s2" };

    Console.Write("Enter your name: ");   // Виведення без переводу рядка

    // NULL-safety — традиція у сучасних мовах програмування, згідно з якою
    // розрізняються типи даних, які дозволяють значення NULL, та ті,
    // які не дозволяють
    string? name = Console.ReadLine();    // Введення з консолі

    if (String.IsNullOrEmpty(name))
    {
        Console.WriteLine("Bye");
    }
    else
    {
        Console.WriteLine("Hello, " + name);   // Виведення з переводом рядка
    }

    // Типи даних
    // Усі типи даних є нащадками загального типу Object
    // Через це вони мають ряд спільних методів: GetType, ToString, GetHashCode, ...
    // Типи данних належать простору імен Systemб для скорочення нструкцій
    //Існують псевдоніми типів:
    int x; // псевдонім для System.Int32
    System.Int32 y;
    string s1;// псевдонім для System.String  
    System.String s2;
    float f; // псевдонім для System.Single -- 32-бітa
    double g; // псевдонім для System.Double -- 64-бітa
              //Nullable - версії
    Nullable<int> v;
    int? a; // скорочена форма Nullable<int>

    Console.Write("How old are you: ");
    String ageInput = Console.ReadLine()!;//!(в кінці) - null-checker 
    int age = int.Parse(ageInput); // перетворення рядка в ціле число
    Console.WriteLine("Next year you`ll be " + (age + 1));

    Console.Write("Previous ages: ");
    for (int i = 0; i < age; i++)
    {
        Console.Write(i + " ");
    }
    int rem = 10 % 3;
    Console.WriteLine();
    char c = ageInput[0]; // отримання першого символу рядка
    ageInput = "A" + ageInput.Substring(1); // заміна першого символу рядка

}

void Problem1()
{
    int even = 0;
    int odd = 0;
    int unique = 0;
    int[] arr = new int[20];
    Random rnd = new();
    for (int i = 0; i < arr.Length; i++)
    {
        arr[i] = rnd.Next(10);
    }
    for (int i = 0; i < arr.Length; i++)
    {
        Console.WriteLine(arr[i]);
    }

    foreach (int el in arr)
    {
        if (el % 2 == 0)
        {
            even += 1;
        }
        else
        {
            odd += 1;
        }
    }
    for (int i = 0; i < arr.Length; i++)
    {
        bool isUnique = true;
        for (int j = 0; j < arr.Length; j++)
        {
            if (i != j && arr[i] == arr[j])
            {
                isUnique = false;
                break;
            }
        }
        if (isUnique)
        {
            unique++;
        }

    }
    Console.WriteLine("Even: " + even);
    Console.WriteLine("Odd: " + odd);
    Console.WriteLine("Unique: " + unique);
}