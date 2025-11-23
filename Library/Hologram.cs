using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Library
{
    internal class Hologram : Literature, INonPrintable
    {
        public string ArtItem { get; set; } = null!;
        public override string GetCard()
        {
            return $"{base.Title}: Hologram of {ArtItem} by {base.Publisher}";
        }

        public string GetNonPrintable()
        {
           return "Non Printable";
        }
        [CiteStyle("IEEE")]
        public void ShowIeee()
        {
            Console.WriteLine($"[\"{Title}\", hologram of {ArtItem}, {Publisher}]");
        }
    }
}
