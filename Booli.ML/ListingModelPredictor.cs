using Booli.ML.Interfaces;
using BooliAPI.Models;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Booli.ML
{
  public class ListingModelPredictor : IPredictor
  {

    private MLContext _mlContext;
    private IList<Listing> _listingsToPredict;
    private string _modelPath;
    private ITransformer _trainedModel;
    private readonly IRepository _repository;

    public ListingModelPredictor(IList<Listing> listingsToPredict, IRepository repository, string modelPath)
    {
      _mlContext = new MLContext();
      _listingsToPredict = listingsToPredict;
      _modelPath = modelPath;
      _repository = repository;
    }

    public void PredictListings()
    {
      LoadModel();
      var predEngine = _mlContext.Model.CreatePredictionEngine<Listing, ListingPrediction>(_trainedModel);
      PrintPredictions(predEngine);
    }

    private void PrintPredictions(PredictionEngine<Listing, ListingPrediction> predictionEngine)
    {

      Console.WriteLine($"*************************************************************************************************************");
      Console.WriteLine($"*       Predictions for housing prices      ");
      Console.WriteLine($"*------------------------------------------------------------------------------------------------------------\r\n\r\n");

      foreach (var listing in _listingsToPredict)
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
        Console.WriteLine($"*       PREDICTED (FUTURE) PRICE:    {pred.SoldPrice}");
        Console.WriteLine($"*************************************************************************************************************\r\n");
      }
    }

    private void LoadModel()
    {
      using (var stream = File.OpenRead(_modelPath))
      {
        _trainedModel = _mlContext.Model.Load(stream, out var inputSchema);
      }
    }
  }
}
