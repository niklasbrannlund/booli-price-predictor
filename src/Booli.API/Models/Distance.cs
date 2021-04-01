using System.Text.Json.Serialization;

namespace Booli.API.Models
{
    public class Distance
    {
        [JsonPropertyName("ocean")] public int Ocean { get; set; }
    }
}