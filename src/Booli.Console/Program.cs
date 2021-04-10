using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using Booli.API.Models;
using Booli.ML.Interfaces;
using BooliAPI;

namespace Booli.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var scope = DependencyInjection.BuildDependencies())
            {
                const string area = "Umeå";
                var repo = scope.Resolve<IRepository>();

                var apiClient = scope.Resolve<IAPIClient>();
                var modelPath = Path.Combine(Environment.CurrentDirectory, "Data/",
                    $"ml_model_{area}.zip");
                
                // get sold listings
                var soldListings = apiClient.GetSoldItemsAsync(area);

                
                // repo.GetPredictionById(soldListings.First().BooliId);
                if (!File.Exists(modelPath))
                {
                    var trainer = scope.Resolve<ITrainer>(new TypedParameter(typeof(IList<SoldListing>), soldListings));
                    trainer.TrainModel();
                    trainer.SaveModel(modelPath);
                }

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