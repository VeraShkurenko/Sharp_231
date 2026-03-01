using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sharp_231.Networking.Orm
{
    internal class NbuRate
    {
        public int R030 { get; set; }
        public String Txt { get; set; } = null!;
        public double Rate { get; set; }
        public String Cc { get; set; } = null!;
        public DateOnly Exchangedate { get; set; }
        public String? Special { get; set; }

        public static NbuRate FromJson(JsonElement jsonElement)
        {
            return new()
            {
                R030 = jsonElement.GetProperty("r030").GetInt32(),
                Txt = jsonElement.GetProperty("txt").GetString()!,
                Rate = jsonElement.GetProperty("rate").GetDouble(),
                Cc = jsonElement.GetProperty("cc").GetString()!,
                Exchangedate = DateOnly.Parse(jsonElement.GetProperty("exchangedate").GetString()!)!,
                Special = jsonElement.GetProperty("special").GetString()
            };
        }

        public override string ToString()
        {
            return $"r030={R030}, txt={Txt}, rate={Rate}, cc={Cc}, exchangedate={Exchangedate}, special={Special}";
        }
    }
}
