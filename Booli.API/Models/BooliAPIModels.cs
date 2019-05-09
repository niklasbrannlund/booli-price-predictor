using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.ML.Data;
using Newtonsoft.Json;

namespace BooliAPI.Models
{
  public class Address
  {

    [JsonProperty("streetAddress")]
    public string StreetAddress { get; set; }
  }

  public class Position
  {

    [JsonProperty("latitude")]
    public double Latitude { get; set; }

    [JsonProperty("longitude")]
    public double Longitude { get; set; }
  }

  public class Region
  {

    [JsonProperty("municipalityName")]
    public string MunicipalityName { get; set; }

    [JsonProperty("countyName")]
    public string CountyName { get; set; }
  }

  public class Distance
  {

    [JsonProperty("ocean")]
    public int Ocean { get; set; }
  }

  public class Location
  {

    [JsonProperty("address")]
    public Address Address { get; set; }

    [JsonProperty("position")]
    public Position Position { get; set; }

    [JsonProperty("namedAreas")]
    public IList<string> NamedAreas { get; set; }

    [JsonProperty("region")]
    public Region Region { get; set; }

    [JsonProperty("distance")]
    public Distance Distance { get; set; }
  }

  public class Source
  {

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }
  }

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
  }

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

    // property to be predicted
    public float SoldPrice => 0;

    public bool HasMissingConstructionYear() => ConstructionYear == 0;

    public bool HasValidConstructionYear(Listing brokenListing) => this.ConstructionYear > 0 && Regex.Replace(brokenListing.Location.Address.StreetAddress, @"[\d-]", string.Empty) ==
                                                                                         Regex.Replace(this.Location.Address.StreetAddress, @"[\d-]", string.Empty);
  }

  public class SearchParams
  {

    [JsonProperty("areaId")]
    public int AreaId { get; set; }
  }

  public class Listings
  {

    [JsonProperty("totalCount")]
    public int TotalCount { get; set; }

    [JsonProperty("count")]
    public int Count { get; set; }

    [JsonProperty("listings")]
    public IList<Listing> CurrentListings { get; set; }

    [JsonProperty("limit")]
    public int Limit { get; set; }

    [JsonProperty("offset")]
    public int Offset { get; set; }

    [JsonProperty("searchParams")]
    public SearchParams SearchParams { get; set; }
  }

  public class Sold
  {

    [JsonProperty("totalCount")]
    public int TotalCount { get; set; }

    [JsonProperty("count")]
    public int Count { get; set; }

    [JsonProperty("sold")]
    public IList<SoldListing> SoldListings { get; set; }

    [JsonProperty("limit")]
    public int Limit { get; set; }

    [JsonProperty("offset")]
    public int Offset { get; set; }

    [JsonProperty("searchParams")]
    public SearchParams SearchParams { get; set; }
  }

}
