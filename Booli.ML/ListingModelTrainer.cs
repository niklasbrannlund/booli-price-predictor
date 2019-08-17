using BooliAPI.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Microsoft.ML.TrainCatalogBase;
using System.Collections.Immutable;

namespace Booli.ML
{
  public class ListingModelTrainer
  {
    private MLContext _mlContext;

    public string ModelPath { get; private set; }

    private IList<SoldListing> _listingDataForTraining;

    public ListingModelTrainer(IList<SoldListing> listingDataForTraining)
    {
      _listingDataForTraining = listingDataForTraining;
      _mlContext = new MLContext();
      ModelPath = Path.Combine(Environment.CurrentDirectory, "Data", $"housing_prediction_model_{DateTime.Today.Month}.zip");
    }

    /// <summary>
    /// Train and save model for predicting housing prices
    /// </summary>
    /// <param name="area">area for which the model should be trained on</param>
    public void TrainAndSaveModel()
    {
      if (!File.Exists(ModelPath))
      {
        var pipeline = ConstructPipelineForTraining();
        var modelAndSchema = TrainModelAndPrintMetrics(pipeline, _listingDataForTraining);
        SaveModelAsFile(modelAndSchema.model, modelAndSchema.inputSchema);
      }
    }

    private EstimatorChain<RegressionPredictionTransformer<Microsoft.ML.Trainers.LinearRegressionModelParameters>> ConstructPipelineForTraining()
    {
      var trainer = _mlContext.Regression.Trainers.Sdca();
      var pipeline = _mlContext.Transforms.Concatenate(outputColumnName: "NumericalFeatures",nameof(SoldListing.ListPrice),
                                                                                             nameof(SoldListing.LivingArea),
                                                                                             nameof(SoldListing.AdditionalArea),
                                                                                             nameof(SoldListing.Rooms),
                                                                                             nameof(SoldListing.ConstructionYear),
                                                                                             nameof(SoldListing.Rent),
                                                                                             nameof(SoldListing.Floor),
                                                                                             nameof(SoldListing.SoldYear))
                                         //.Append(_mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "CategoricalFeatures", nameof(SoldListing.ObjectType)))
                                         .Append(_mlContext.Transforms.Concatenate("Features", "NumericalFeatures"))
                                         .Append(trainer);

      return pipeline;
    }

    private (ITransformer model, DataViewSchema inputSchema) TrainModelAndPrintMetrics(EstimatorChain<RegressionPredictionTransformer<Microsoft.ML.Trainers.LinearRegressionModelParameters>> pipeline, IList<SoldListing> houseDataForTraining)
    {
      var dataView = _mlContext.Data.LoadFromEnumerable(houseDataForTraining);
      var model = pipeline.Fit(dataView);
      var metrics = _mlContext.Regression.CrossValidate(data: dataView, estimator: pipeline, numberOfFolds: 4, labelColumnName: "Label");

      PrintRegressionFoldsAverageMetrics(metrics);

      return (model, dataView.Schema);
    }

    private void SaveModelAsFile(ITransformer model, DataViewSchema schema)
    {
      using (var fileStream = new FileStream(ModelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
        _mlContext.Model.Save(model, schema, fileStream);
    }

    private static void PrintRegressionFoldsAverageMetrics(IReadOnlyList<CrossValidationResult<RegressionMetrics>> crossValidationResults)
    {
      var RMS = crossValidationResults.Select(r => r.Metrics.RootMeanSquaredError);
      var lossFunction = crossValidationResults.Select(r => r.Metrics.LossFunction);
      var R2 = crossValidationResults.Select(r => r.Metrics.RSquared);

      Console.WriteLine($"*************************************************************************************************************");
      Console.WriteLine($"*       Metrics for Regression model      ");
      Console.WriteLine($"*------------------------------------------------------------------------------------------------------------");
      Console.WriteLine($"*       Average RMS:          {RMS.Average():0.###}  ");
      Console.WriteLine($"*       Average Loss Function: {lossFunction.Average():0.###}  ");
      Console.WriteLine($"*       Average R-squared: {R2.Average():0.###}  ");
      Console.WriteLine($"*************************************************************************************************************");
    }
  }
}
