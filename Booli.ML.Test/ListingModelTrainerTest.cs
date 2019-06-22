using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BooliAPI;
using BooliAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Booli.ML.Test
{
  [TestClass]
  public class ListingModelTrainerTest
  {
    private IAPIClient _client;
    private List<SoldListing> _listingsForTraining;

    [TestInitialize]
    public void Setup()
    {
      _listingsForTraining = new List<SoldListing>();

      var apiClientMock = new Mock<IAPIClient>();
      _client = apiClientMock.Object;
    }

    [TestMethod]
    public void CanConstruct()
    {
      // Arrange & Act
      var modeltrainer = new ListingModelTrainer(_listingsForTraining);

      // Assert
      Assert.IsNotNull(modeltrainer);
    }

    [TestMethod]
    public void VerifyCorrectNameOfModel()
    {
      // Arrange
      var modeltrainer = new ListingModelTrainer(_listingsForTraining);
      var currentMonth = DateTime.Today.Month;
      
      // Act
      var modelName = ExtractModelNameFromPath(modeltrainer.ModelPath);

      // Assert
      Assert.AreEqual($"housing_prediction_model_{currentMonth}.zip", modelName, "Wrong model name");
    }


    private string ExtractModelNameFromPath(string pathForModelFile)
    {
      Regex regex = new Regex(@"[\w-]+(?:\.\w+)*$");
      return regex.Match(pathForModelFile).Value;
    }
  }
}
