using Booli.API.Models;
using System.Collections.Generic;

namespace BooliAPI
{
    public interface IAPIClient
    {
        IList<SoldListing> GetSoldItemsAsync(string area);
        IList<Listing> GetListingsAsync(string area);
    }
}