using System.Text.Json.Serialization;

namespace Booli.API.Models
{
    public class Address
    {
        [JsonPropertyName("streetAddress")] public string StreetAddress { get; set; }
    }
}