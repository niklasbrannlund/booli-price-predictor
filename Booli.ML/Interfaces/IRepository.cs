using BooliAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booli.ML.Interfaces
{
  public interface IRepository
  {
    void SaveListing(Listing listing);
    void SavePrediction();
  }
}
