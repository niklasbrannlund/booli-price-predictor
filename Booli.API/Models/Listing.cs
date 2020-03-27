using Microsoft.ML.Data;
using Newtonsoft.Json;
using System;
using LiteDB;
using System.Device.Location;
using System.Text.RegularExpressions;

namespace BooliAPI.Models
{
  public class Listing
  {
    [NoColumn]
    [BsonId]
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

    // return the distance from listing to city centre (km)
    // City centre coordinates is here taken to be rådhustorget
    public float DistanceToCityCentre
    {
      get
      {
        var cityCentreLat = 63.825910;
        var cityCentreLong = 20.263166;

        var cityCentrCoord = new GeoCoordinate(cityCentreLat, cityCentreLong);
        var listingCoord = new GeoCoordinate(this.Location.Position.Latitude, this.Location.Position.Longitude);

        return (float)Math.Round(listingCoord.GetDistanceTo(cityCentrCoord)/1000);

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

    [ColumnName("Label")]
    public float SoldPrice { get; set; }
  }
}
