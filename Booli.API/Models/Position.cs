using Newtonsoft.Json;

namespace BooliAPI.Models
{
  public class Position
  {
    [JsonProperty("latitude")]
    public double Latitude { get; set; }

    [JsonProperty("longitude")]
    public double Longitude { get; set; }
  }
}
