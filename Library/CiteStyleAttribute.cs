using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Library
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class CiteStyleAttribute:Attribute
    {
        public string Style { get; }

        public CiteStyleAttribute(string style)
        {
            Style = style;
        }
    }
}
