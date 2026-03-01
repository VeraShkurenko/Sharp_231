using Sharp_231.Networking.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sharp_231.Networking.Api
{
    internal class NbuApi
    {
        public static List<NbuRate> ListFromJsonString(String json)
        {
            return [..
                JsonSerializer.Deserialize<JsonElement>(json)
                .EnumerateArray()
                .Select(NbuRate.FromJson)
                ];
        }
    }
}
