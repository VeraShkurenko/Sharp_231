using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Library
{
    internal class Poster:Literature, INonPrintable
    {
        public String Subject { get; set; }

        public override string GetCard()
            {
                return $"{base.Title} - '{Subject}' - {base.Publisher}";
            }

        public string GetNonPrintable()
        {
            return "Non Printable";
        }
        [CiteStyle("IEEE")]
        public void ShowIeee()
        {
            Console.WriteLine($"[Poster: \"{Title}\" ({Subject}), {Publisher}]");
        }

    }
}
