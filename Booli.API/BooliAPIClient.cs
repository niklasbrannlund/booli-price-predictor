using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BooliAPI;
using BooliAPI.Models;
using Newtonsoft.Json;

namespace Booli.API
{
  /// <summary>
  /// Client for calls to Booli API
  /// </summary>
  public class BooliApiClient : IAPIClient
  {
    private string apiKey_;
    private string callerId_;
    protected const string baseUrl_ = "https://api.booli.se/";

    public BooliApiClient(string apikey, string callerId)
    {
      apiKey_ = apikey;
      callerId_ = callerId;
    }

    public async Task<Sold> GetSoldItemsAsync(string area)
    {
      string result = "";
      var soldUrl = baseUrl_ + $"/sold?q={area}" + CreateAuthentication();
      Sold parsedObject = null;
      try
      {
        using (var client = new HttpClient() { Timeout = TimeSpan.FromMilliseconds(500) })
        {
          result = await client.GetStringAsync(soldUrl);
          parsedObject = JsonConvert.DeserializeObject<Sold>(result);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Exception caught: {ex.Message}");
      }
      return parsedObject;
    }

    public async Task<Listings> GetListingsAsync(string area)
    {
      string result = "";
      var listingUrl = baseUrl_ + $"/listings?q={area}" + CreateAuthentication();
      Listings parsedObject = null;
      try
      {
        using (var client = new HttpClient() { Timeout = TimeSpan.FromMilliseconds(500) })
        {
          result = await client.GetStringAsync(listingUrl);
          parsedObject = JsonConvert.DeserializeObject<Listings>(result);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Exception caught: {ex.Message}");
      }
      return parsedObject;
    }

    private string CreateAuthentication()
    {
      var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
      var unique = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16);
      string result = "";
      using (var sha = new SHA1Managed())
      {
        var input = Encoding.UTF8.GetBytes(callerId_ + time + apiKey_ + unique);
        var hash = sha.ComputeHash(input);
        result = string.Concat(hash.Select(b => b.ToString("x2")));
      }

      return $"&limit=450&offset=0&callerId={callerId_}&time={time}&unique={unique}&hash={result}";
    }
  }
}
