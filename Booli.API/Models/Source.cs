using Newtonsoft.Json;

namespace BooliAPI.Models
{
  public class Source
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }
  }
}
