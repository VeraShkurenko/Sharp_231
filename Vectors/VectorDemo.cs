using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Vectors
{
    internal class VectorDemo
    {
        public void Run()
        {
            Console.WriteLine("Vector Demo");
            Vector v1 = new() { X = 10, Y = 20 };
            Vector v2 = v1;//перевірка семантики
            v1.X = 30; //чи відіб'ється зміна v1 на v2?

            Console.WriteLine("v1.x = {0}, v2.x = {1}", v1.X, v2.X);
            //різні об'єкти v1.x = 30, v2.x = 10

            //повторюємо для реферонсного типу
            RefVector r1 = new() { X = 10, Y = 20 }; ;
            RefVector r2 = r1;
            r1.X = 30;
            Console.WriteLine("r1.x = {0}, r2.x = {1}", r1.X, r2.X);
            //один об'єкт, зміни синхронізовані r1.x = 30, r2.x = 30
            // оператори з новим типом 
            Console.WriteLine("+v1 = {0}", +v1);
            Console.WriteLine("v1 + v2 = {0}", v1 + v2);
            Console.WriteLine("-v1 = {0}", -v1);
            Console.WriteLine("v1 - v2 = {0}", v1 - v2);
            Vector zero = new Vector(0, 0);
            //оператори true/false виникають тоді, коли об'єкт є умовою, зокрема у тернарному виразі
            Console.WriteLine("zero vector is " + (zero ? "true" : "false"));//zero vector is false
            //оператор ! визначається окремо
            Console.WriteLine("v1 vector is " + (v1 ? "true" : "false"));//v1 vector is true
           //оператор інверсії "~" визначений як відбитий від осі Y
            Console.WriteLine("~v1 = {0}", ~v1);//~v1 = (-30.0000; -20.0000)
            Console.WriteLine("v1++=>{0}", ++v1);//v1++=>(31.0000; 21.0000)
            Console.WriteLine("v1--=>{0}", --v1);//v1--=>(30.0000; 20.0000)
            v1 += v2;
            Console.WriteLine("v1+=v2=>{0}", v1);
            Console.WriteLine("v1 * v2 = {0}", v1 * v2);
            Console.WriteLine("v1 / v2 = {0}", v1 / v2);
            Console.WriteLine("v1 % v2 = {0}", v1 % v2);
            Console.WriteLine("v1 << 2 = {0}", v1 << 2);
            Console.WriteLine("v1 >> 2 = {0}", v1 >> 2);
            Console.WriteLine("v1 >>> 2 = {0}", v1 >>> 2);
            Console.WriteLine("v1 & v2 = {0}", v1 & v2);
            Console.WriteLine("v1 | v2 = {0}", v1 | v2);
            Console.WriteLine("v1 ^ v2 = {0}", v1 ^ v2);

            Console.WriteLine("v1 == v2 => {0}", v1 == v2);
            Console.WriteLine("v1 != v2 => {0}", v1 != v2);
            Console.WriteLine("v1 > v2  => {0}", v1 > v2);
            Console.WriteLine("v1 < v2  => {0}", v1 < v2);
            Console.WriteLine("v1 >= v2 => {0}", v1 >= v2);
            Console.WriteLine("v1 <= v2 => {0}", v1 <= v2);

        }
    }
}
/*Особливості роботи з Value Types 
 - базовими типами з семантикою "за значенням" є struct та record struct


 */
