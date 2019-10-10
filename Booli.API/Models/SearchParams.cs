using Newtonsoft.Json;

namespace BooliAPI.Models
{
  public class SearchParams
  {
    [JsonProperty("areaId")]
    public int AreaId { get; set; }
  }
}
