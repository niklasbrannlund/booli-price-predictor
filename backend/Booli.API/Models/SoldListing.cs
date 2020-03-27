using Microsoft.ML.Data;
using Newtonsoft.Json;
using System;
using System.Device.Location;

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
