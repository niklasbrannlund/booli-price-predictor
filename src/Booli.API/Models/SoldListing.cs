// using Microsoft.ML.Data;

using System;
using System.Text.Json.Serialization;
using Microsoft.ML.Data;

namespace Booli.API.Models
{
    public class SoldListing
    {
        [NoColumn]
        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("constructionYear")] public float ConstructionYear { get; set; }

        [JsonPropertyName("listPrice")] public float ListPrice { get; set; }

        [JsonPropertyName("rent")] public float Rent { get; set; }

        [JsonPropertyName("floor")] public float Floor { get; set; }

        [JsonPropertyName("livingArea")] public float LivingArea { get; set; }

        [NoColumn]
        [JsonPropertyName("source")]
        public Source Source { get; set; }

        [JsonPropertyName("rooms")] public float Rooms { get; set; }

        [NoColumn]
        [JsonPropertyName("published")]
        public string Published { get; set; }

        [JsonPropertyName("objectType")] public string ObjectType { get; set; }

        [NoColumn]
        [JsonPropertyName("booliId")]
        public int BooliId { get; set; }

        [NoColumn]
        [JsonPropertyName("soldDate")]
        public string SoldDate { get; set; }

        public float SoldYear
        {
            get
            {
                var date = DateTime.Parse(SoldDate);
                return date.Year;
            }
        }
        
        public float DistanceToCityCentre => Utils.GetDistanceToCityCentre(Location.Position.Latitude, Location.Position.Longitude);

        [JsonPropertyName("soldPrice")]
        [ColumnName("Label")]
        public float SoldPrice { get; set; }

        [NoColumn]
        [JsonPropertyName("soldPriceSource")]
        public string SoldPriceSource { get; set; }

        [NoColumn] [JsonPropertyName("url")] public string Url { get; set; }

        [JsonPropertyName("additionalArea")] public float AdditionalArea { get; set; }

        [NoColumn]
        [JsonPropertyName("apartmentNumber")]
        public string ApartmentNumber { get; set; }

        [JsonPropertyName("plotArea")] public float PlotArea { get; set; }
    }
}