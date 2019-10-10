using Microsoft.ML.Data;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace BooliAPI.Models
{
  public class Listing
  {
    [NoColumn]
    [JsonProperty("booliId")]
    public int BooliId { get; set; }

    [JsonProperty("listPrice")]
    public float ListPrice { get; set; }

    [JsonProperty("published")]
    public string Published { get; set; }

    public float SoldYear
    {
      get
      {
        var date = DateTime.Parse(Published);
        return date.Year;
      }
    }

    [NoColumn]
    [JsonProperty("source")]
    public Source Source { get; set; }

    [JsonProperty("floor")]
    public float Floor { get; set; }

    [NoColumn]
    [JsonProperty("location")]
    public Location Location { get; set; }

    [JsonProperty("objectType")]
    public string ObjectType { get; set; }

    [JsonProperty("rooms")]
    public float Rooms { get; set; }

    [JsonProperty("livingArea")]
    public float LivingArea { get; set; }

    [JsonProperty("additionalArea")]
    public float AdditionalArea { get; set; }

    [JsonProperty("plotArea")]
    public float PlotArea { get; set; }

    [JsonProperty("constructionYear")]
    public float ConstructionYear { get; set; }

    [NoColumn]
    [JsonProperty("url")]
    public string Url { get; set; }

    [NoColumn]
    [JsonProperty("isNewConstruction")]
    public float? IsNewConstruction { get; set; }

    [JsonProperty("rent")]
    public float Rent { get; set; }

    public bool HasMissingConstructionYear() => ConstructionYear == 0;

    public bool HasValidConstructionYear(Listing brokenListing) => this.ConstructionYear > 0 && Regex.Replace(brokenListing.Location.Address.StreetAddress, @"[\d-]", string.Empty) ==
                                                                                         Regex.Replace(this.Location.Address.StreetAddress, @"[\d-]", string.Empty);
  }

  public class ListingPrediction
  {
    [ColumnName("Score")]
    public float SoldPrice;
  }
}
