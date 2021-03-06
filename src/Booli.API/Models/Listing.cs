﻿using System;
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
        
        public float DistanceToCityCentre => Utils.GetDistanceToCityCentre(Location.Position.Latitude, Location.Position.Longitude);
        
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