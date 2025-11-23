using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Exeptions
{
    internal class ExceptionsDemo
    {
        public ExceptionsDemo()
        {
            // конструктор - метод без повернення
            return; //переривання конструктора не перериває побудови об'єкта
            //єдини спосіб зупинити побудову об'єкта - створити виняток
        }

        public void Run()
        {
            Console.WriteLine("Exсeptions Demo");
            while (true)
            {
                Console.Write("Введіть число для обчислення кореня: ");
                string str = Console.ReadLine();
                try
                {
                    Console.WriteLine("Sqrt of {0} = {1}", str, SqrtFromString(str));
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Аргумент не може бути перетворений до числа");
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Неможливо обчислити корінь з від'ємного числа");
                }
                catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException)
                {
                    Console.WriteLine("Зафіксовано null або порожній рядок в аргументі");
                }
                Console.WriteLine("Cпробуйте знову.\n");
            }
        }
        public void Run1()
        {
            Console.WriteLine("Exсeptions Demo");
            //this.ThroablaCode();//у .Net дозволяється лишати виклик
            //коду з винятками без обробників(у деяких мовах - ні)
            //у такій формі виняток завершує роботу всієї програми
            try
            {
                this.ThrowableCode();
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine("Exсeption: "+ ex.Message);
                return;

            }
            catch (IOException)//блоків catch може бути декілька
            {                 //перевырка сумісності іде з гори до низу
                Console.WriteLine("Unexpected exсeption ");
            }
            catch // всі види винятків з ігноруванням об'єкту винятку
            {
                //якщо виняток не обробляється, рекомендується передпати його далі
                throw;
            }
            finally//виконується в будь-якому разі
            {      // навіть якщо є команда return
                Console.WriteLine("Finally executed");
            }
        }
        private void ThrowableCode()
        {
            //створення виняткової ситуації
            throw new LiteratureParseException("Unrecognisable literature type");
        }
        private double SqrtFromString(string str)
        {
            ArgumentNullException.ThrowIfNull(str);//скоротчена форма 
            //if (str == null)
            //{
            //    throw ArgumentNullException(nameof(str));
            //}
            str =str.Trim();
            if(str ==String.Empty)
            {
                throw new ArgumentException("Blank or empty data passed");
            }
            double result;
            try 
            {
                result = Double.Parse(str); 
            }
            catch
            {
                throw new ArgumentOutOfRangeException(nameof(str),"Argument must be valid float namber");
            }
            if (result<0)
            {
                throw new InvalidOperationException("Negative values unsupported");
            }
            return Math.Sqrt(result);
        }
    }
}

/*
 Винятки (Exceptions)
 Спосіб організації процесу виконання коду за якого процес може бути
 зупинений та переведений до режиму оброблення винятку.
 
у ранніх мовах програмування винятків не було, перевірка помилок
здійснювалась через запит спеціальної функції накштальт last_error()

в ооп з'являються ситуації, які неможна зупинити засобами return 
- конструктори об'єктів є методами без повернення.
Вимагається інший спосіб переривання роботи

Механізм: 
- виняткова ситуація утворюється командою throw, яка приймає 
  об'єкт-виняток, який передається обробникам
- поява винятку зупиняє виконання коду з місця команди throw і відбувається
  пошук найближчого обробника. якщо він знайдений, то управління передається до нього
  якщо  не знайдений, то весь процес зупиняється в аварійному режимі
- обробники формуються блоками catch або finally

Рекомендації:
- використовувати максимально конкретні реалізації винятків
- не використовувати загальні накштальт Exception, SystemException
- використовувати блок finally для прибирання ресурсів
- розподіляти логіку, не перевантажувати один блок try великою кількістю задач
- розподіляти логіку обробників винятків - обробляти лише ті з них, 
  які належать до відповідальності данного логічного блоку
  method() {date ----> get_by_date(date){query}}
                                           |
                                       обробляємо винятки по query
                                        не обробляємо по date
- якщо у програмі є точка входу, то бажано її оточити 
  обробником для непередбачення ситуації
- не слід використовувати винятки для організації логіки алгоритмів
  (наприклад, для визначення сусідніх елементів масиву
  (у крайніх елементів лише по одному сусіду, в решти - по два))
    
  for(i=0,...)
        try{arr[i-1]}
        catch(IndexOutOfRangeException){ignore}
- семантика винятку - проблема, яку не можна вирішити на місці

- якщо ми розробляємо бібліотеку або великий сервіс, 
  то є сенс створювати власні винятки, замість стандартних системних 

 */
