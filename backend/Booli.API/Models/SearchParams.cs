
using System.Text.Json.Serialization;

namespace Booli.API.Models
{
  public class SearchParams
  {
    [JsonPropertyName("areaId")]
    public int AreaId { get; set; }
  }
}
