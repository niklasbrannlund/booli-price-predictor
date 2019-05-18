using System;
using System.Collections.Generic;
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

    [TestInitialize]
    public void Setup()
    {
      var apiClientMock = new Mock<IAPIClient>();
      _client = apiClientMock.Object;
    }

    [TestMethod]
    public void CanConstruct()
    {
      var currentListings = new List<SoldListing>();
      var modeltrainer = new ListingModelTrainer(currentListings);
      Assert.IsNotNull(modeltrainer);
    }
  }
}
