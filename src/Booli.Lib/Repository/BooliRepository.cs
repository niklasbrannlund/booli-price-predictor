using Booli.ML.Interfaces;
using Booli.API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using LiteDB;

namespace Booli.Lib
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

        public void SavePrediction(Listing prediction)
        {
            using (var db = new LiteDatabase($@"{Directory.GetCurrentDirectory()}\Data\predictions.db"))
            {
              db.GetCollection<Listing>().Upsert(prediction);
            }
        }

        public Listing GetPredictionById(int id)
        {
            using (var db = new LiteDatabase($@"{Directory.GetCurrentDirectory()}\Data\predictions.db"))
            {
              return db.GetCollection<Listing>().FindById(new BsonValue(id));
            }
        }

        public IEnumerable<Listing> GetPredictions()
        {
            using (var db = new LiteDatabase($@"{Directory.GetCurrentDirectory()}\Data\predictions.db"))
            {
              return db.GetCollection<Listing>().FindAll();
            }
        }
    }
}