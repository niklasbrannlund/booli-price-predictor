using System;
using System.Collections.Generic;
using System.Globalization;
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
                var apiClient = scope.Resolve<IAPIClient>();
                var modelPath = Path.Combine(Environment.CurrentDirectory, "Data/",
                    $"model_{DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture)}.zip");
                const string area = "Umeå";
                if (!File.Exists(modelPath))
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