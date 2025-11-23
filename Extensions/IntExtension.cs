using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Extensions
{
    //клас розширення який додає функціональність типу int 
    public static class IntExtension//клас має бути public static
    {
        //метод має бути public static з позначкою типу на
        //який він поширюється з модифікатором this
        public static String px(this Int32 value)
        {
            return $"{value}px";
        }
        public static String percnt(this Int32 value)
        {
            return $"{value}%";
        }

    }
}
