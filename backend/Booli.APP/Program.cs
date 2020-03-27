using Booli.ML.Interfaces;
using BooliAPI;
using Autofac;
using BooliAPI.Models;
using System.Collections.Generic;
using System.IO;
using System;
using System.Globalization;

namespace Booli.APP
{
  class Program
  {
    static void Main(string[] args)
    {
      using (var scope = DependencyInjection.BuildDependencies())
      {
        var apiClient = scope.Resolve<IAPIClient>();
        var modelPath = Path.Combine(Environment.CurrentDirectory, "Data/", $"model_{DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture)}.zip");
        const string area = "Umeå";
        if(!File.Exists(modelPath))
        {
          var soldListings = apiClient.GetSoldItemsAsync(area);
          var trainer = scope.Resolve<ITrainer>(new TypedParameter(typeof(IList<SoldListing>), soldListings));
          trainer.TrainModel();
          trainer.SaveModel(modelPath);
        }

        var repo = scope.Resolve<IRepository>();
        var listingsToPredict = apiClient.GetListingsAsync(area);

        var predictor = scope.Resolve<IPredictor>(new TypedParameter(typeof(IList<Listing>), listingsToPredict),
                                                  new TypedParameter(typeof(IRepository), repo),
                                                  new TypedParameter(typeof(string), modelPath));

        predictor.CreatePredictionEngine();
        predictor.Predict();
        predictor.SavePredictions();
      }
    }
  }
}
