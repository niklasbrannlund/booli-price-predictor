using Booli.API.Models;
using System.Collections.Generic;

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