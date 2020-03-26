using Microsoft.ML.Data;
using Newtonsoft.Json;
using System;

namespace BooliAPI.Models
{
  public class SoldListing
  {
    [NoColumn]
    [JsonProperty("location")]
    public Location Location { get; set; }

    [JsonProperty("constructionYear")]
    public float ConstructionYear { get; set; }

    [JsonProperty("listPrice")]
    public float ListPrice { get; set; }

    [JsonProperty("rent")]
    public float Rent { get; set; }

    [JsonProperty("floor")]
    public float Floor { get; set; }

    [JsonProperty("livingArea")]
    public float LivingArea { get; set; }

    [NoColumn]
    [JsonProperty("source")]
    public Source Source { get; set; }

    [JsonProperty("rooms")]
    public float Rooms { get; set; }

    [NoColumn]
    [JsonProperty("published")]
    public string Published { get; set; }

    [JsonProperty("objectType")]
    public string ObjectType { get; set; }

    [NoColumn]
    [JsonProperty("booliId")]
    public int BooliId { get; set; }

    [NoColumn]
    [JsonProperty("soldDate")]
    public string SoldDate { get; set; }

    public float SoldYear
    {
      get
      {
        var date = DateTime.Parse(this.SoldDate);
        return date.Year;
      }
    }

    [JsonProperty("soldPrice")]
    [ColumnName("Label")]
    public float SoldPrice { get; set; }

    [NoColumn]
    [JsonProperty("soldPriceSource")]
    public string SoldPriceSource { get; set; }

    [NoColumn]
    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("additionalArea")]
    public float AdditionalArea { get; set; }

    [NoColumn]
    [JsonProperty("apartmentNumber")]
    public string ApartmentNumber { get; set; }

    [JsonProperty("plotArea")]
    public float PlotArea { get; set; }
  }
}
