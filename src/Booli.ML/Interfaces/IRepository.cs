using Booli.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booli.API.Models;

namespace Booli.ML.Interfaces
{
  public interface IRepository
  {
    void SaveListing(Listing listing);
    void SaveListings(IEnumerable<Listing> listings);
    void SavePrediction(Listing prediction);
    Listing GetPredictionById(int id);
    IEnumerable<Listing> GetPredictions();
  }
}
