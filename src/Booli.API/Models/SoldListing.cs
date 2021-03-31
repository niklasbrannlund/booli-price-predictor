// using Microsoft.ML.Data;
using System;
using System.Text.Json.Serialization;

namespace Booli.API.Models
{
  public class SoldListing
  {
    // [NoColumn]
    [JsonPropertyName("location")]
    public Location Location { get; set; }

    [JsonPropertyName("constructionYear")]
    public float ConstructionYear { get; set; }

    [JsonPropertyName("listPrice")]
    public float ListPrice { get; set; }

    [JsonPropertyName("rent")]
    public float Rent { get; set; }

    [JsonPropertyName("floor")]
    public float Floor { get; set; }

    [JsonPropertyName("livingArea")]
    public float LivingArea { get; set; }

    // [NoColumn]
    [JsonPropertyName("source")]
    public Source Source { get; set; }

    [JsonPropertyName("rooms")]
    public float Rooms { get; set; }

    // [NoColumn]
    [JsonPropertyName("published")]
    public string Published { get; set; }

    [JsonPropertyName("objectType")]
    public string ObjectType { get; set; }

    // [NoColumn]
    [JsonPropertyName("booliId")]
    public int BooliId { get; set; }

    // [NoColumn]
    [JsonPropertyName("soldDate")]
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

        var cityCentrCoord = (cityCentreLat, cityCentreLong);
        var listingCoord = (this.Location.Position.Latitude, this.Location.Position.Longitude);

        return (float)CalculateDistance(cityCentrCoord, listingCoord);

      }
    }
    
    public double CalculateDistance((double lat, double lon) point1, (double lat, double lon) point2)
        {
            var d1 = point1.lat * (Math.PI / 180.0);
            var num1 = point1.lon * (Math.PI / 180.0);
            var d2 = point2.lat * (Math.PI / 180.0);
            var num2 = point2.lon * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

    [JsonPropertyName("soldPrice")]
    // [ColumnName("Label")]
    public float SoldPrice { get; set; }

    // [NoColumn]
    [JsonPropertyName("soldPriceSource")]
    public string SoldPriceSource { get; set; }

    // [NoColumn]
    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("additionalArea")]
    public float AdditionalArea { get; set; }

    // [NoColumn]
    [JsonPropertyName("apartmentNumber")]
    public string ApartmentNumber { get; set; }

    [JsonPropertyName("plotArea")]
    public float PlotArea { get; set; }
  }
}
