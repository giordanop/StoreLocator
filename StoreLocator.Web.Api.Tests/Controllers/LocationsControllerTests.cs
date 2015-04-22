using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using Moq;
using NUnit.Framework;
using StoreLocator.Domain.Dto;
using StoreLocator.Domain.Repository;
using StoreLocator.Web.Api.Controllers;
using StoreLocator.Web.Api.Models;

namespace StoreLocator.Web.Api.Tests.Controllers
{
    [TestFixture]
    public class LocationsControllerTests
    {
        private HttpRequestMessage request;
        private HttpConfiguration config;
        [SetUp]
        public void Initialize()
        {
            config = new HttpConfiguration();
            request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/");
            var route = config.Routes.MapHttpRoute("Default", "api/{controller}/{id}");
        }


        [Test]
        public async void GetLocations_not_null_value_should_return_search_model_object()
        {
            
            var locations = new List<LocationDto>()
            {
                new LocationDto() {Latitude = 30, Longitude = 40},
                new LocationDto() {Latitude = 30, Longitude = 40},
                new LocationDto() {Latitude = 30, Longitude = 40}
            };

            double latitude = 10;
            double longitude = 20;
            int maxResults = 10;
            int maxDistance = 10;
                
            var repository = new Mock<ILocationRepository>();
            repository
                .Setup(r => r.GetLocations(It.IsAny<LocationDto>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(locations);

            var controller = new LocationsController(repository.Object, new ModelFactory())
            {
                Request = request
            };
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            //act
            var searchResult = controller.Get(latitude, longitude, maxResults, maxDistance);

            //assert
            Assert.IsNotNull(searchResult);
            Assert.AreEqual(HttpStatusCode.OK,searchResult.StatusCode);

            var result = await searchResult.Content.ReadAsAsync<SearchResultModel>();

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.LocationsFound.Count());
            Assert.AreEqual(longitude, result.OriginalLocation.Longitude);
            Assert.AreEqual(latitude, result.OriginalLocation.Latitude);
            Assert.AreEqual(maxDistance, result.MaxDistance);
            Assert.AreEqual(maxResults, result.MaxResults);
            repository.Verify(m => m.GetLocations(It.IsAny<LocationDto>(), It.IsAny<int>(), It.IsAny<int>()),Times.Once);
        }


        [Test]
        public async void GetLocations_no_results_should_return_search_model_object_locations_empty()
        {

            var locations = new List<LocationDto>();

            double latitude = 10;
            double longitude = 20;
            int maxResults = 10;
            int maxDistance = 10;

            var repository = new Mock<ILocationRepository>();
            repository
                .Setup(r => r.GetLocations(It.IsAny<LocationDto>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(locations);

            var controller = new LocationsController(repository.Object, new ModelFactory())
            {
                Request = request
            };
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            //act
            var searchResult = controller.Get(latitude, longitude, maxResults, maxDistance);

            //assert
            Assert.IsNotNull(searchResult);

            var result = await searchResult.Content.ReadAsAsync<SearchResultModel>();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.LocationsFound);
            Assert.AreEqual(0, result.LocationsFound.Count());
            Assert.AreEqual(longitude, result.OriginalLocation.Longitude);
            Assert.AreEqual(latitude, result.OriginalLocation.Latitude);
            Assert.AreEqual(maxDistance, result.MaxDistance);
            Assert.AreEqual(maxResults, result.MaxResults);
            repository.Verify(m => m.GetLocations(It.IsAny<LocationDto>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async void GetLocations_zero_results_should_not_calling_repository_function()
        {

            var locations = new List<LocationDto>();

            double latitude = 10;
            double longitude = 20;
            int maxResults = 0;
            int maxDistance = 10;

            var repository = new Mock<ILocationRepository>();
            repository
                .Setup(r => r.GetLocations(It.IsAny<LocationDto>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(locations);

            var controller = new LocationsController(repository.Object, new ModelFactory())
            {
                Request = request
            };
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;

            //act
            var searchResult = controller.Get(latitude, longitude, maxResults, maxDistance);

            //assert
            Assert.IsNotNull(searchResult);

            var result = await searchResult.Content.ReadAsAsync<SearchResultModel>();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.LocationsFound);
            Assert.AreEqual(0, result.LocationsFound.Count());
            repository.Verify(m => m.GetLocations(It.IsAny<LocationDto>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}
