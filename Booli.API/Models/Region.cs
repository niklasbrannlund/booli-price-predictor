using Newtonsoft.Json;

namespace BooliAPI.Models
{
  public class Region
  {

    [JsonProperty("municipalityName")]
    public string MunicipalityName { get; set; }

    [JsonProperty("countyName")]
    public string CountyName { get; set; }
  }
}
