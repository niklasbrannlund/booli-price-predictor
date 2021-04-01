using System;
using System.Text.Json.Serialization;
using Microsoft.ML.Data;

namespace Booli.API.Models
{
    public class Listing
    {
        [JsonPropertyName("booliId")] public int BooliId { get; set; }

        [JsonPropertyName("listPrice")] public float ListPrice { get; set; }

        [JsonPropertyName("published")] public string Published { get; set; }

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
        public float DistanceToCityCentre =>
            // var cityCentreLat = 63.825910;
            // var cityCentreLong = 20.263166;
            //
            // var cityCentrCoord = new GeoCoordinate(cityCentreLat, cityCentreLong);
            // var listingCoord = new GeoCoordinate(this.Location.Position.Latitude, this.Location.Position.Longitude);
            //
            // return (float)Math.Round(listingCoord.GetDistanceTo(cityCentrCoord)/1000);
            (float) 0.0;

        [NoColumn]
        [JsonPropertyName("source")]
        public Source Source { get; set; }

        [JsonPropertyName("floor")] public float Floor { get; set; }

        [NoColumn]
        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("objectType")] public string ObjectType { get; set; }

        [JsonPropertyName("rooms")] public float Rooms { get; set; }

        [JsonPropertyName("livingArea")] public float LivingArea { get; set; }

        [JsonPropertyName("additionalArea")] public float AdditionalArea { get; set; }

        [JsonPropertyName("plotArea")] public float PlotArea { get; set; }

        [JsonPropertyName("constructionYear")] public float ConstructionYear { get; set; }

        [NoColumn] [JsonPropertyName("url")] public string Url { get; set; }

        [NoColumn]
        [JsonPropertyName("isNewConstruction")]
        public float? IsNewConstruction { get; set; }

        [JsonPropertyName("rent")] public float Rent { get; set; }

        [ColumnName("Label")] public float SoldPrice { get; set; }
    }
}