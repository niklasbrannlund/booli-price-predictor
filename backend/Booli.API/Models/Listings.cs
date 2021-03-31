using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Booli.API.Models
{
  public class Listings
  {
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("listings")]
    public IList<Listing> CurrentListings { get; set; }

    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    [JsonPropertyName("offset")]
    public int Offset { get; set; }

    [JsonPropertyName("searchParams")]
    public SearchParams SearchParams { get; set; }
  }
}
