using Newtonsoft.Json;

namespace BooliAPI.Models
{
  public class Distance
  {
    [JsonProperty("ocean")]
    public int Ocean { get; set; }
  }
}
