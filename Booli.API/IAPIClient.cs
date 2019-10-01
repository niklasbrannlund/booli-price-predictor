using BooliAPI.Models;

namespace BooliAPI
{
  public interface IAPIClient
  {
    Sold GetSoldItemsAsync(string area);
    Listings GetListingsAsync(string area);
  }
}
