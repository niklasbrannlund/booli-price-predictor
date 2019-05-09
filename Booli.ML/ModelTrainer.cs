
using BooliAPI;
using BooliAPI.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.ML.TrainCatalogBase;

namespace Booli.ML
{
  public class ModelTrainer
  {
    private MLContext mlContext_;
    private readonly IAPIClient client_;
    private string modelPath_;
    private readonly string area_;
    public ModelTrainer(IAPIClient client, string area)
    {
      mlContext_ = new MLContext();
      client_ = client;
      area_ = area;
      modelPath_ = Path.Combine(Environment.CurrentDirectory, "Data", $"housing_prediction_model_{new GregorianCalendar().GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstDay, DayOfWeek.Monday)}.zip");
    }

    /// <summary>
    /// Train and save model for predicting housing prices
    /// </summary>
    /// <param name="area">area for which the model should be trained on</param>
    public void GetTrainingDataAndTrainModel()
    {
      if (!File.Exists(modelPath_))
      {
        var currentListings = GetDataForTraining().Result;
        var pipeline = ConstructPipelineForTraining();
        var model = TrainModelAndPrintMetrics(pipeline, currentListings);
        SaveModelAsFile(model);
      }
    }

    private ITransformer TrainModelAndPrintMetrics(EstimatorChain<RegressionPredictionTransformer<FastTreeTweedieModelParameters>> pipeline, IList<SoldListing> houseDataForTraining)
    {
      var dataView = mlContext_.Data.LoadFromEnumerable(houseDataForTraining);
      var model = pipeline.Fit(dataView);
      var metrics = mlContext_.Regression.CrossValidate(data: dataView, estimator: pipeline, numFolds: 4, labelColumn: "Label");

      PrintRegressionFoldsAverageMetrics(metrics);
      return model;
    }

    private async Task<IList<SoldListing>> GetDataForTraining()
    {
      var soldItems = await client_.GetSoldItemsAsync(area_);
      return soldItems.SoldListings;
    }

    private EstimatorChain<RegressionPredictionTransformer<FastTreeTweedieModelParameters>> ConstructPipelineForTraining()
    {
      var trainer = mlContext_.Regression.Trainers.FastTreeTweedie(labelColumnName: DefaultColumnNames.Label, featureColumnName: DefaultColumnNames.Features);
      var pipeline = mlContext_.Transforms.Concatenate(outputColumnName: "NumericalFeatures", nameof(SoldListing.ListPrice),
                                                                                             nameof(SoldListing.LivingArea),
                                                                                             nameof(SoldListing.AdditionalArea),
                                                                                             nameof(SoldListing.Rooms),
                                                                                             nameof(SoldListing.ConstructionYear),
                                                                                             nameof(SoldListing.Rent),
                                                                                             nameof(SoldListing.Floor),
                                                                                             nameof(SoldListing.SoldYear))
                                         .Append(mlContext_.Transforms.Categorical.OneHotEncoding(outputColumnName: "CategoricalFeatures", nameof(SoldListing.ObjectType)))
                                         .Append(mlContext_.Transforms.Concatenate(outputColumnName: DefaultColumnNames.Features, "NumericalFeatures", "CategoricalFeatures"))
                                         .Append(mlContext_.Transforms.CopyColumns(outputColumnName: DefaultColumnNames.Label, nameof(SoldListing.SoldPrice)))
                                         .Append(trainer);

      return pipeline;
    }

    public void EvaluateCurrentListings()
    {
      var request = client_.GetListingsAsync(area_);
      request.Wait();
      var currentListings = request.Result.CurrentListings;
      
      PreProcessData(currentListings);

      ITransformer trainedModel;
      using (var stream = File.OpenRead(modelPath_))
      {
        trainedModel = mlContext_.Model.Load(stream);
      }

      var predEngine = mlContext_.Model.CreatePredictionEngine<Listing, ListingPrediction>(trainedModel);
      PrintPrediction(predEngine, currentListings);

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

    private void PrintPrediction(PredictionEngine<Listing, ListingPrediction> predictionEngine, IList<Listing> currentListings)
    {

      Console.WriteLine($"*************************************************************************************************************");
      Console.WriteLine($"*       Predictions for housing prices in {area_}      ");
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

    private void SaveModelAsFile(ITransformer model)
    {
      using (var fileStream = new FileStream(modelPath_, FileMode.Create, FileAccess.Write, FileShare.Write))
        mlContext_.Model.Save(model, fileStream);
    }

    private static void PrintRegressionFoldsAverageMetrics(IReadOnlyList<CrossValidationResult<RegressionMetrics>> crossValidationResults)
    {
      var L1 = crossValidationResults.Select(r => r.Metrics.L1);
      var L2 = crossValidationResults.Select(r => r.Metrics.L2);
      var RMS = crossValidationResults.Select(r => r.Metrics.Rms);
      var lossFunction = crossValidationResults.Select(r => r.Metrics.LossFn);
      var R2 = crossValidationResults.Select(r => r.Metrics.RSquared);

      Console.WriteLine($"*************************************************************************************************************");
      Console.WriteLine($"*       Metrics for Regression model      ");
      Console.WriteLine($"*------------------------------------------------------------------------------------------------------------");
      Console.WriteLine($"*       Average L1 Loss:    {L1.Average():0.###} ");
      Console.WriteLine($"*       Average L2 Loss:    {L2.Average():0.###}  ");
      Console.WriteLine($"*       Average RMS:          {RMS.Average():0.###}  ");
      Console.WriteLine($"*       Average Loss Function: {lossFunction.Average():0.###}  ");
      Console.WriteLine($"*       Average R-squared: {R2.Average():0.###}  ");
      Console.WriteLine($"*************************************************************************************************************");
    }

    private async Task<IList<SoldListing>> FetchTrainingDataAsync(string area)
    {
      // retrieve the data
      var soldResult = await client_.GetSoldItemsAsync(area);
      var soldListings = soldResult.SoldListings;
      return soldListings;
    }
  }

  public class ListingPrediction
  {
    public float Score;
  }

  
}
