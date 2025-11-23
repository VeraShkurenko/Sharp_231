using Sharp_231.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Library
{
    internal class Journal : Literature, IPeriodic
    {
        public String Number { get; set; } = null;

        public override string GetCard()
        {
            return $"{base.Title} - {Number} - {base.Publisher}";
        }
        public string GetPeriod()
        {
            return "Month";
        }

        [CiteStyle("IEEE")]
        public void ShowIeee()
        {
            Console.WriteLine($"[\"{Title}\", no. {Number}, {Publisher}]");
        }
    }
}
