using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Vectors
{
    internal struct Vector
    //struct - базовий тип для Value Type
    {
        public double X { get; set; }
        public double Y { get; set; }
        //параметризований конструктор
        public Vector(double x, double y) { X = x; Y = y; }
        public override string? ToString() { return $"({X:F4}; {Y:F4})"; }
        #region
        public static Vector operator +(Vector a) => new() { X = a.X, Y = a.Y };
        public static Vector operator -(Vector a) => new() { X = -a.X, Y = -a.Y };
        public static Vector operator +(Vector a, Vector b) => new() { X = a.X + b.X, Y = a.Y + b.Y };
        public static Vector operator -(Vector a, Vector b) => new() { X = a.X - b.X, Y = a.Y - b.Y };
        public static bool operator true(Vector a) => a.X != 0 || a.Y != 0;
        public static bool operator false(Vector a) => a.X == 0 && a.Y == 0;
        public static bool operator !(Vector a) => a.X == 0 && a.Y == 0;
        public static Vector operator ~(Vector a) => new() { X = -a.X, Y = a.Y };
        public static Vector operator ++(Vector a) => new() { X = a.X + 1, Y = a.Y + 1 };
        public static Vector operator --(Vector a) => new() { X = a.X - 1, Y = a.Y - 1 };

        //нові (від С# 14) скорочені оператори(+=,-=) означаються для об'єктів(не static)
        //до 14-го не перевантажується, а створюється автоматично
        public static double operator *(Vector a, Vector b) =>  a.X * b.X+a.Y * b.Y ;
        public static double operator /(Vector a, Vector b) => a * ~b;
        public double this[int index]
        {
            get => index switch
            {
                0 => X,
                1 => Y,
                _ => throw new IndexOutOfRangeException("1 or only")
            };
            set
            {
                switch (index)
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;

                    default: throw new IndexOutOfRangeException("0 or only");
                }
            }
        }
        public double this[String index]
        {
            get => index.ToLower() switch
            {
                "x" => X,
                "y" => Y,
                _ => throw new IndexOutOfRangeException("1 or only")
            };
            set
            {
                switch (index.ToLower())
                {
                    case "x": X = value; break;
                    case "y": Y = value; break;
                    default: throw new IndexOutOfRangeException("0 or only");
                }
            }
        }

        // Оператор залишку від ділення
        public static Vector operator %(Vector a, Vector b) => new() { X = a.X % b.X, Y = a.Y % b.Y, };

        // Оператор бітового зсуву вліво (<<)
        public static Vector operator <<(Vector a, int n) => new() { X = a.X - n, Y = a.Y };

        // Оператор бітового зсуву вправо (>>)
        public static Vector operator >>(Vector a, int n) => new() { X = a.X + n, Y = a.Y };
        public static Vector operator >>>(Vector a, int n) => new() { X = a.X + n, Y = -a.Y };
        public static Vector operator &(Vector a, Vector b) => new() { X = Math.Max(a.X, b.X), Y = Math.Max(a.Y, b.Y) };
        public static Vector operator |(Vector a, Vector b) => new() { X = Math.Min(a.X, b.X), Y = Math.Min(a.Y, b.Y) };
        public static Vector operator ^(Vector a, Vector b) => new() { X = Math.Abs(Math.Min(a.X, b.X)), Y = Math.Abs(Math.Min(a.Y, b.Y)) };
        // оператори порівняння та відношення- мають оголошення парами (<,>),(==,!=),(<=,>=)
        public static bool operator ==(Vector a, Vector b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Vector a, Vector b) => !(a== b);
        public static bool operator >(Vector a, Vector b) => a.X>b.X && a.Y>b.Y;
        public static bool operator <(Vector a, Vector b) => a.X<b.X && a.Y<b.Y;
        public static bool operator >=(Vector a, Vector b) => a.X >= b.X && a.Y >= b.Y;
        public static bool operator <=(Vector a, Vector b) => a.X <= b.X && a.Y <= b.Y;
        #endregion
    }
}
