using System.Text.Json.Serialization;

namespace Booli.API.Models
{
    public class Source
    {
        [JsonPropertyName("name")] public string Name { get; set; }

        [JsonPropertyName("id")] public int Id { get; set; }

        [JsonPropertyName("type")] public string Type { get; set; }

        [JsonPropertyName("url")] public string Url { get; set; }
    }
}