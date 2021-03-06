using System.Collections.Generic;
using Booli.API.Models;
using BooliAPI;
using Moq;
using NUnit.Framework;

namespace Booli.ML.Test
{
    public class ListingModelTrainerTest
    {
        private IAPIClient _client;
        private List<SoldListing> _listingsForTraining;
        [SetUp]
        public void Setup()
        {
            _listingsForTraining = new List<SoldListing>();

            var apiClientMock = new Mock<IAPIClient>();
            _client = apiClientMock.Object;
        }

        [Test]
        public void CanConstruct()
        {
            // Arrange & Act
            var modeltrainer = new ListingModelTrainer(_listingsForTraining);

            // Assert
            Assert.IsNotNull(modeltrainer);
        }
    }
}