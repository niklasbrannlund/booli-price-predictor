
using BooliAPI;
using BooliAPI.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.FastTree;
using Microsoft.Data.DataView;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
      ModelPath = Path.Combine(Environment.CurrentDirectory, "Data", $"housing_prediction_model_{new GregorianCalendar().GetWeekOfYear(DateTime.Today, CalendarWeekRule.FirstDay, DayOfWeek.Monday)}.zip");
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
        var model = TrainModelAndPrintMetrics(pipeline, _listingDataForTraining);
        SaveModelAsFile(model);
      }
    }

    private EstimatorChain<RegressionPredictionTransformer<Microsoft.ML.Trainers.LinearRegressionModelParameters>> ConstructPipelineForTraining()
    {
      var trainer = _mlContext.Regression.Trainers.StochasticDualCoordinateAscent(labelColumnName: DefaultColumnNames.Label, featureColumnName: DefaultColumnNames.Features);
      var pipeline = _mlContext.Transforms.Concatenate(outputColumnName: "NumericalFeatures",nameof(SoldListing.ListPrice),
                                                                                             nameof(SoldListing.LivingArea),
                                                                                             nameof(SoldListing.AdditionalArea),
                                                                                             nameof(SoldListing.Rooms),
                                                                                             nameof(SoldListing.ConstructionYear),
                                                                                             nameof(SoldListing.Rent),
                                                                                             nameof(SoldListing.Floor),
                                                                                             nameof(SoldListing.SoldYear))
                                         .Append(_mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "CategoricalFeatures", nameof(SoldListing.ObjectType)))
                                         .Append(_mlContext.Transforms.Concatenate(outputColumnName: DefaultColumnNames.Features, "NumericalFeatures", "CategoricalFeatures"))
                                         .Append(trainer);

      return pipeline;
    }

    private ITransformer TrainModelAndPrintMetrics(EstimatorChain<RegressionPredictionTransformer<Microsoft.ML.Trainers.LinearRegressionModelParameters>> pipeline, IList<SoldListing> houseDataForTraining)
    {
      var dataView = _mlContext.Data.LoadFromEnumerable(houseDataForTraining);
      var model = pipeline.Fit(dataView);
      var metrics = _mlContext.Regression.CrossValidate(data: dataView, estimator: pipeline, numFolds: 4, labelColumn: "Label");

      PrintRegressionFoldsAverageMetrics(metrics);

      return model;
    }

    private void SaveModelAsFile(ITransformer model)
    {
      using (var fileStream = new FileStream(ModelPath, FileMode.Create, FileAccess.Write, FileShare.Write))
        _mlContext.Model.Save(model, fileStream);
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

    private void PrintFeatureImportanceValues(IDataView dataView)
    {
      var featureColumnNames = dataView.GetFeatureColumnNames();

      var pipeline = _mlContext.Transforms.Concatenate(DefaultColumnNames.Features, featureColumnNames)
                    .Append(_mlContext.Transforms.Normalize(DefaultColumnNames.Features))
                    .Append(_mlContext.Regression.Trainers.StochasticDualCoordinateAscent());

      var model = pipeline.Fit(dataView);
      var preprocessedTrainingData = model.Transform(dataView);

      ImmutableArray<RegressionMetricsStatistics> permutationFeatureImportance =
                                                  _mlContext
                                                  .Regression
                                                  .PermutationFeatureImportance(model.LastTransformer, preprocessedTrainingData, permutationCount: 3);

      // Order features by importance
      var featureImportanceMetrics =
          permutationFeatureImportance
              .Select((metric, index) => new { index, metric.RSquared })
              .OrderByDescending(myFeatures => Math.Abs(myFeatures.RSquared.Mean));

      Console.WriteLine("Feature\tPFI");

      foreach (var feature in featureImportanceMetrics)
      {
        Console.WriteLine($"{featureColumnNames[feature.index],-20}|\t{feature.RSquared.Mean:F6}");
      }
    }
  }
}
