using Newtonsoft.Json;
using System.Collections.Generic;

namespace BooliAPI.Models
{
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
