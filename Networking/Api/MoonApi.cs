using SharpKnP321.Networking.Orm;
using System.Text.Json;

namespace SharpKnP321.Networking.Api
{
    internal class MoonApi
    {
        public async Task<MoonPhase> PhaseByDateAsync(DateOnly date)
        {
            var moonApiResponse = await FetchDataAsync(date.Year, date.Month, date.Day);
            return moonApiResponse.Phase[date.Day.ToString()];
        }

        private async Task<MoonApiResponse> FetchDataAsync(int year, int month, int day)
        {
            using HttpClient httpClient = new();
            // Параметр day в URL вказує на початок календаря, але API повертає весь місяць
            string href = $"https://www.icalendar37.net/lunar/api/?year={year}&month={month}&day={day}&shadeColor=gray&size=150&texturize=true";
            string json = await httpClient.GetStringAsync(href);
            return JsonSerializer.Deserialize<MoonApiResponse>(json)!;
        }
    }
}
