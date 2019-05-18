using BooliAPI.Models;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booli.ML
{
  public class ListingModelPredictor
  {

    private MLContext _mlContext;
    private IList<Listing> _listingsToPredict;
    private string _modelPath;

    public ListingModelPredictor(IList<Listing> listingsToPredict, string modelPath)
    {
      _mlContext = new MLContext();
      _listingsToPredict = listingsToPredict;
      _modelPath = modelPath;
    }

    public void PredictListings()
    {
      ITransformer trainedModel;
      using (var stream = File.OpenRead(_modelPath))
      {
        trainedModel = _mlContext.Model.Load(stream);
      }

      var predEngine = _mlContext.Model.CreatePredictionEngine<Listing, ListingPrediction>(trainedModel);
      PrintPrediction(predEngine, _listingsToPredict);

    }

    private void PrintPrediction(PredictionEngine<Listing, ListingPrediction> predictionEngine, IList<Listing> currentListings)
    {

      Console.WriteLine($"*************************************************************************************************************");
      Console.WriteLine($"*       Predictions for housing prices      ");
      Console.WriteLine($"*------------------------------------------------------------------------------------------------------------\r\n\r\n");

      foreach (var listing in currentListings)
      {
        var pred = predictionEngine.Predict(listing);
        Console.WriteLine($"*       Address:                     {listing.Location.Address.StreetAddress} ");
        Console.WriteLine($"*       Type:                        {listing.ObjectType} ");
        Console.WriteLine($"*       Listing price:               {listing.ListPrice} ");
        Console.WriteLine($"*       Living area:                 {listing.LivingArea} m^2");
        Console.WriteLine($"*       Additional area:             {listing.AdditionalArea} m^2");
        Console.WriteLine($"*       Rooms:                       {listing.Rooms}");
        Console.WriteLine($"*       Floor:                       {listing.Floor}");
        Console.WriteLine($"*       Rent:                        {listing.Rent}");
        Console.WriteLine($"*       Construction year:           {listing.ConstructionYear}");
        Console.WriteLine($"*       PREDICTED (FUTURE) PRICE:    {pred.Score}");
        Console.WriteLine($"*************************************************************************************************************\r\n");
      }
    }

    private void PreProcessData(IList<Listing> currentListings)
    {
      if (currentListings.Any(x => x.HasMissingConstructionYear()))
      {
        var listingsWithoutConstructionYear = currentListings.Where(x => x.HasMissingConstructionYear());
        CopyConstructionYearFromListingOnSameAddress(currentListings, listingsWithoutConstructionYear);
      }

    }

    private static void CopyConstructionYearFromListingOnSameAddress(IList<Listing> currentListings, IEnumerable<Listing> listingsWithoutConstructionYear)
    {
      foreach (var brokenListing in listingsWithoutConstructionYear)
      {
        brokenListing.ConstructionYear = currentListings.FirstOrDefault(x => x.HasValidConstructionYear(brokenListing)).ConstructionYear;
      }
    }
  }
  public class ListingPrediction
  {
    public float Score;
  }
}
