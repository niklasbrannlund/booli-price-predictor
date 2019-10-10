using Booli.API;
using Booli.ML;
using Booli.ML.Interfaces;
using System.Configuration;

namespace Booli.APP
{
  class Program
  {
    static void Main(string[] args)
    {
      var client = new BooliApiClient(ConfigurationManager.AppSettings["ApiKey"], ConfigurationManager.AppSettings["CallerId"]);

      var listingsForTraining = client.GetSoldItemsAsync("svedmyra");
      ListingModelTrainer trainer = new ListingModelTrainer(listingsForTraining.SoldListings);
      trainer.TrainAndSaveModel();

      var booliRepo = new BooliRepository();

      var listingtToPredict = client.GetListingsAsync("svedmyra");
      var predictor = new ListingModelPredictor(listingtToPredict.CurrentListings, booliRepo, trainer.ModelPath);
      predictor.PredictListings();
    }
  }
}
