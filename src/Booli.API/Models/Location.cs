using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Booli.API.Models
{
  public class Location
  {
    [JsonPropertyName("address")]
    public Address Address { get; set; }

    [JsonPropertyName("position")]
    public Position Position { get; set; }

    [JsonPropertyName("namedAreas")]
    public IList<string> NamedAreas { get; set; }

    [JsonPropertyName("region")]
    public Region Region { get; set; }

    [JsonPropertyName("distance")]
    public Distance Distance { get; set; }
  }
}
