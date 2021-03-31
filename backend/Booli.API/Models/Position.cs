using System.Text.Json.Serialization;

namespace Booli.API.Models
{
  public class Position
  {
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }
  }
}
