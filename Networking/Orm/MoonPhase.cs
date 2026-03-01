using System.Text.Json.Serialization;

namespace SharpKnP321.Networking.Orm
{
    internal class MoonPhase
    {
        [JsonPropertyName("phaseName")]
        public string PhaseName { get; set; } = null!;

        [JsonPropertyName("lighting")]
        public double Lighting { get; set; }

        [JsonPropertyName("svg")]
        public string Svg { get; set; } = null!;

        [JsonPropertyName("npWidget")]
        public string NpWidget { get; set; } = null!;

    }
}