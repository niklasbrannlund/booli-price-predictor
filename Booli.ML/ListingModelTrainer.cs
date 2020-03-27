using Booli.ML.Interfaces;
using BooliAPI.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Booli.ML
{
  public class ListingModelTrainer : ITrainer
  {
    private MLContext _mlContext;
    private IList<SoldListing> _listingDataForTraining;
    private ITransformer _model;

    public ListingModelTrainer(IList<SoldListing> listingDataForTraining)
    {
      _listingDataForTraining = listingDataForTraining;
      _mlContext = new MLContext();
    }

    /// <summary>
    /// Train and save model for predicting housing prices
    /// </summary>
    /// <param name="area">area for which the model should be trained on</param>
    public void TrainModel()
    {
      var pipeline = ConstructPipelineForTraining();
      _model = TrainModelAndPrintMetrics(pipeline, _listingDataForTraining);
    }

    private IEstimator<ITransformer> ConstructPipelineForTraining()
    {
      var trainer = _mlContext.Regression.Trainers.FastTree();
      var pipeline = _mlContext.Transforms
        .Categorical.OneHotEncoding(outputColumnName: "ObjectTypeEncoded", inputColumnName: nameof(SoldListing.ObjectType))
        .Append(_mlContext.Transforms.Concatenate("Features", "ObjectTypeEncoded",
                                                              nameof(SoldListing.ListPrice),
                                                              nameof(SoldListing.DistanceToCityCentre),
                                                              nameof(SoldListing.LivingArea),
                                                              nameof(SoldListing.AdditionalArea),
                                                              nameof(SoldListing.Rooms),
                                                              nameof(SoldListing.ConstructionYear),
                                                              nameof(SoldListing.Rent),
                                                              nameof(SoldListing.Floor),
                                                              nameof(SoldListing.SoldYear),
                                                              nameof(SoldListing.PlotArea)))
        .Append(trainer);

      return pipeline;
    }

    private ITransformer TrainModelAndPrintMetrics(IEstimator<ITransformer> pipeline, IList<SoldListing> houseDataForTraining)
    {
      var dataView = _mlContext.Data.LoadFromEnumerable(houseDataForTraining);
      var split = _mlContext.Data.TrainTestSplit(dataView);
      var model = pipeline.Fit(split.TrainSet);
      

      var predictions = model.Transform(split.TestSet);
      var metrics = _mlContext.Regression.Evaluate(predictions);
      PrintRegressionFoldsAverageMetrics(metrics);

      return model;
    }

    public void SaveModel(string modelPath)
    {
      Directory.CreateDirectory(Path.GetDirectoryName(modelPath));
      using (var fileStream = new FileStream(modelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
        _mlContext.Model.Save(_model, _mlContext.Data.LoadFromEnumerable(_listingDataForTraining).Schema, fileStream);
    }

    private static void PrintRegressionFoldsAverageMetrics(RegressionMetrics metrics)
    {
      var RMS = metrics.RootMeanSquaredError;
      var lossFunction = metrics.LossFunction;
      var R2 = metrics.RSquared;

      Console.WriteLine($"*************************************************************************************************************");
      Console.WriteLine($"*       Metrics for Regression model      ");
      Console.WriteLine($"*------------------------------------------------------------------------------------------------------------");
      Console.WriteLine($"*       Average RMS:          {RMS:0.###}  ");
      Console.WriteLine($"*       Average Loss Function: {lossFunction:0.###}  ");
      Console.WriteLine($"*       Average R-squared: {R2:0.###}  ");
      Console.WriteLine($"*************************************************************************************************************");
    }
  }
}
