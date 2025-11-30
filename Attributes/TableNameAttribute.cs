using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Attributes;

[AttributeUsage(AttributeTargets.Class)]

internal class TableNameAttribute(String value) : Attribute
{
    public String Value { get; init; } = value;
}