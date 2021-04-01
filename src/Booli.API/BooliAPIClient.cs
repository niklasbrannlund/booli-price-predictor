using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Booli.API.Models;
using BooliAPI;

namespace Booli.API
{
    public class BooliApiClient : IAPIClient
    {
        private string _apiKey;
        private string _callerId;
        protected const string _baseUrl = "https://api.booli.se/";

        public BooliApiClient(string apikey, string callerId)
        {
            _apiKey = apikey;
            _callerId = callerId;
        }

        public IList<SoldListing> GetSoldItemsAsync(string area)
        {
            var limit = 400;
            var offset = 0;
            var listings = new List<SoldListing>();
            var totalNumOfListings = 0;
            do
            {
                var soldUrl = _baseUrl + $"/sold?q={area}" + CreateAuthentication(limit, offset);
                var result = MakeRequestAsync<Sold>(soldUrl).Result;
                totalNumOfListings = result.TotalCount;
                listings.AddRange(result.SoldListings);
                offset += limit;
            } while (listings.Count < totalNumOfListings);

            return listings.ToList();
        }

        public IList<Listing> GetListingsAsync(string area)
        {
            var listingUrl = _baseUrl + $"/listings?q={area}" + CreateAuthentication();
            var result = MakeRequestAsync<Listings>(listingUrl).Result;
            return result.CurrentListings.ToList();
        }


        private async Task<T> MakeRequestAsync<T>(string url)
        {
            var result = "";
            T parsedObject = default;
            using (var client = new HttpClient() {Timeout = TimeSpan.FromMilliseconds(10000)})
            {
                result = await client.GetStringAsync(url);
                parsedObject = System.Text.Json.JsonSerializer.Deserialize<T>(result);
            }

            return parsedObject;
        }

        private string CreateAuthentication(int limit = 400, int offset = 0)
        {
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var unique = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16);
            var result = "";
            using (var sha = new SHA1Managed())
            {
                var input = Encoding.UTF8.GetBytes(_callerId + time + _apiKey + unique);
                var hash = sha.ComputeHash(input);
                result = string.Concat(hash.Select(b => b.ToString("x2")));
            }

            return $"&limit={limit}&offset={offset}&callerId={_callerId}&time={time}&unique={unique}&hash={result}";
        }
    }
}