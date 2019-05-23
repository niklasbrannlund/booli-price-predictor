using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooliAPI.Models;

namespace BooliAPI
{
  public interface IAPIClient
  {
    Sold GetSoldItemsAsync(string area);
    Listings GetListingsAsync(string area);
  }
}
