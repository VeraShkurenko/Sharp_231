using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Fractions
{
    internal class FractionDemo
    {
        public void Run()
        {
            Console.WriteLine("Fraction Demo");

            Fraction f1 = new(1, 2);
            Fraction f2 = new(2, 3);

            Console.WriteLine("f1 = {0}, f2 = {1}", f1, f2);


            Console.WriteLine("f1 + f2 = {0}", f1 + f2);
            Console.WriteLine("f1 - f2 = {0}", f1 - f2);
            Console.WriteLine("f1 * f2 = {0}", f1 * f2);
            Console.WriteLine("f1 / f2 = {0}", f1 / f2);

            Console.WriteLine("f1 == f2 => {0}", f1 == f2);
            Console.WriteLine("f1 != f2 => {0}", f1 != f2);
            Console.WriteLine("f1 > f2  => {0}", f1 > f2);
            Console.WriteLine("f1 < f2  => {0}", f1 < f2);
            Console.WriteLine("f1 >= f2 => {0}", f1 >= f2);
            Console.WriteLine("f1 <= f2 => {0}", f1 <= f2);
    }
    }
}
