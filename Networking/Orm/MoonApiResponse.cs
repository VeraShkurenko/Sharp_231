using System.Text.Json.Serialization;

namespace SharpKnP321.Networking.Orm
{
    internal class MoonApiResponse
    {
        [JsonPropertyName("phase")]
        public Dictionary<string, MoonPhase> Phase { get; set; } = [];
    }
}