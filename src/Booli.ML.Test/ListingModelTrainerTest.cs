using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
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
    }
}