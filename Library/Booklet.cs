using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharp_231.Library
{
    internal class Booklet : Literature
    {
        public String Author { get; set; } = null!;
        public string Subject { get; set; }

        public override string GetCard()
        {
            return $"{Author}, {base.Title} - {Subject}";
        }

        public void Print()
        {
            Console.WriteLine("Printing... " + GetCard());
        }

        [ColorPrint(Copies =2)]//ending '...Attribute' optional
        public void PrintWithColor(String colorScheme)
        {
            Console.WriteLine($"In colors '{colorScheme}' printing... " + GetCard());

        }
        [CiteStyle("IEEE")]
        public void ShowIeee()
        {
            Console.WriteLine($"[{Author}, \"{Title}: {Subject}\", {Publisher}]");
        }
    }
}
