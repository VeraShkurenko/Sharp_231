using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Exeptions
{
    /// <summary>
    /// Represents an error that occurs during the parsing of literature data.
    /// </summary>
    internal class LiteratureParseException(String message) : Exception(message)
    {
        //primary constructor - оголошується біля самого класу 

    }
}
