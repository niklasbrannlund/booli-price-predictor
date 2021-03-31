
using System.Text.Json.Serialization;

namespace Booli.API.Models
{
  public class Region
  {

    [JsonPropertyName("municipalityName")]
    public string MunicipalityName { get; set; }

    [JsonPropertyName("countyName")]
    public string CountyName { get; set; }
  }
}
