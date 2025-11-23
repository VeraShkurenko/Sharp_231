using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Extensions
{
    public static class DoubleExtension
    {
        public static String toMoney(this Double value)
        {
            return Math.Round(value, 2).ToString("F2");
        }
    }
}
