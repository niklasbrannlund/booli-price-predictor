using System;
using BooliAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Booli.ML.Test
{
  [TestClass]
  public class ModelTrainerTest
  {
    private IAPIClient _client;
    private string _area;

    [TestInitialize]
    public void Setup()
    {
      var apiClientMock = new Mock<IAPIClient>();
      _client = apiClientMock.Object;

      _area = "svedmyra";
    }

    [TestMethod]
    public void CanConstruct()
    {
      var modeltrainer = new ModelTrainer(_client, _area);
      Assert.IsNotNull(modeltrainer);
    }
  }
}
