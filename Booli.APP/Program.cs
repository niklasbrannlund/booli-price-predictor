using Booli.API;
using Booli.ML;
using Booli.ML.Interfaces;
using BooliAPI;
using Autofac;
using System.Configuration;
using BooliAPI.Models;
using System.Collections.Generic;

namespace Booli.APP
{
  class Program
  {
    static void Main(string[] args)
    {
      using (var scope = DependencyInjection.BuildDependencies())
      {
        var apiClient = scope.Resolve<IAPIClient>();

        const string area = "svedmyra";
        var soldListingsResponse = apiClient.GetSoldItemsAsync(area);
        var trainer = scope.Resolve<ITrainer>(new TypedParameter(typeof(IList<SoldListing>), soldListingsResponse.SoldListings));
        trainer.TrainModel();

        var repo = scope.Resolve<IRepository>();
        var listingsToPredict = apiClient.GetListingsAsync(area);

        var predictor = scope.Resolve<IPredictor>(new TypedParameter(typeof(IList<Listing>), listingsToPredict.CurrentListings),
                                                  new TypedParameter(typeof(IRepository), repo),
                                                  new TypedParameter(typeof(string), trainer.ModelPath));

        predictor.CreatePredictionEngine();
        predictor.Predict();
        predictor.SavePredictions();
      }
    }
  }
}
