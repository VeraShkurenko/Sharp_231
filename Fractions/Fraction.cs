using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Fractions
{
    internal struct Fraction
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Fraction(double x, double y)
        {
            if (y == 0)
                throw new DivideByZeroException("Denominator cannot be zero");

            X = x;
            Y = y;
            Reduce();
        }
        private void Reduce()
        {
            double g = GCD(Math.Abs(X), Math.Abs(Y));
            X /= g;
            Y /= g;

            if (Y < 0)
            {
                X = -X;
                Y = -Y;
            }
        }
        private static double GCD(double a, double b)
        {
            while (b != 0)
                (a, b) = (b, a % b);
            return a;
        }
        public override string ToString() => $"{X}/{Y}";

        public static Fraction operator +(Fraction a, Fraction b) =>
          new(a.X * b.Y + b.X * a.Y, a.Y * b.Y);

        public static Fraction operator -(Fraction a, Fraction b) =>
            new(a.X * b.Y - b.X * a.Y, a.Y * b.Y);

        public static Fraction operator *(Fraction a, Fraction b) =>
            new(a.X * b.X, a.Y * b.Y);

        public static Fraction operator /(Fraction a, Fraction b) =>
       new(a.X * b.Y, a.Y * b.X);

        public static bool operator ==(Fraction a, Fraction b) =>
            a.X == b.X && a.Y == b.Y;

        public static bool operator !=(Fraction a, Fraction b) =>
            !(a == b);

        public static bool operator >(Fraction a, Fraction b) =>
    a.X * b.Y > b.X * a.Y;

        public static bool operator <(Fraction a, Fraction b) =>
            a.X * b.Y < b.X * a.Y;

        public static bool operator >=(Fraction a, Fraction b) =>
            a.X * b.Y >= b.X * a.Y;

        public static bool operator <=(Fraction a, Fraction b) =>
            a.X * b.Y <= b.X * a.Y;

    }

}
