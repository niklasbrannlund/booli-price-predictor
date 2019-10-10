using Newtonsoft.Json;
using System.Collections.Generic;

namespace BooliAPI.Models
{
  public class Location
  {
    [JsonProperty("address")]
    public Address Address { get; set; }

    [JsonProperty("position")]
    public Position Position { get; set; }

    [JsonProperty("namedAreas")]
    public IList<string> NamedAreas { get; set; }

    [JsonProperty("region")]
    public Region Region { get; set; }

    [JsonProperty("distance")]
    public Distance Distance { get; set; }
  }
}
