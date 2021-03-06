using System.Collections.Generic;
using Booli.API.Models;
using Booli.ML.Interfaces;
using BooliAPI;
using Moq;
using NUnit.Framework;

namespace Booli.ML.Test
{
    public class ListingModelPredictorTest
    {
        
        private Mock<IAPIClient> _apiClientMock;
        private Mock<IRepository> _repositoryMock;
        private IList<Listing> _listingsToPredict;
        
        [SetUp]
        public void Setup()
        {
            _listingsToPredict = new List<Listing>()
            {
                new Listing()
                {
                    BooliId = 1,
                    AdditionalArea = 0,
                    ConstructionYear = 2018,
                    Floor = 2,
                    ListPrice = 2500000,
                    LivingArea = 45,
                    Location = new Location()
                    {
                        Address = new Address() {StreetAddress = "Oppundavagen 27"}
                    },
                    Rent = 4000,
                    Rooms = 2
                }
            };

            _apiClientMock = new Mock<IAPIClient>();
            _repositoryMock = new Mock<IRepository>();
        }

        [Test]
        public void CanConstruct()
        {
            var predictor = new ListingModelPredictor(_listingsToPredict, _repositoryMock.Object, null);
            Assert.IsNotNull(predictor, "Construction of predictor failed ");
        }
    }
}