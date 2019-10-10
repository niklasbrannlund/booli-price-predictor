using Booli.ML.Interfaces;
using BooliAPI.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;

namespace Booli.ML
{
  public class BooliRepository : IRepository
  {

    private readonly string listingsDBPath = Path.Combine(Environment.CurrentDirectory, $"/Data/");

    public void SaveListing(Listing listing)
    {
      using (var db = new LiteDatabase($@"{listingsDBPath}\listings.db"))
      {
       db.GetCollection<Listing>("listings").Insert(listing);
      }
    }

    public void SaveListings(IEnumerable<Listing> listings)
    {
      using (var db = new LiteDatabase($@"{listingsDBPath}\listings.db"))
      {
        db.GetCollection<Listing>("listings").Insert(listings);
      }
    }

    public void SavePrediction(ListingPrediction prediction)
    {
      using (var db = new LiteDatabase($@"{Directory.GetCurrentDirectory()}\Data\predictions.db"))
      {
        db.GetCollection<ListingPrediction>().Insert(prediction);
      }
    }
  }
}
