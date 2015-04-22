using System.Net;
using System.Net.Http;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using StoreLocator.Web.Api.Models;

namespace StoreLocator.Web.Api.Tests.Controllers
{
    [TestFixture]
    public class LocationsControllerIntegrationTests
    {

        [SetUp]
        public void Initialize()
        {
            
        }


        [Test]
        public async void GetLocations_call_with_real_params_Should_return_Ok()
        {
            using (var server = TestServer.Create<Startup>())
            {
                using (var client = new HttpClient(server.Handler))
                {
                    var response = await client.GetAsync("http://localhost:12701/api/locations/?latitude=50&longitude=5&maxResults=10&maxDistance=130000");

                    var result = await response.Content.ReadAsAsync<SearchResultModel>();
                    Assert.AreEqual(HttpStatusCode.OK,response.StatusCode);
                    Assert.IsNotNull(result.LocationsFound);
                }
            }
        }

        [Test]
        public async void GetLocations_call_with_some_null_params_Should_return_bad()
        {
            using (var server = TestServer.Create<Startup>())
            {
                using (var client = new HttpClient(server.Handler))
                {
                    var response = await client.GetAsync("http://localhost:12701/api/locations/?latitude=&longitude=&maxResults=10&maxDistance=130000");

                    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

                }
            }
        }

        [Test]
        public async void GetLocations_call_with_some_negative_params_Should_return_bad()
        {
            using (var server = TestServer.Create<Startup>())
            {
                using (var client = new HttpClient(server.Handler))
                {
                    var response = await client.GetAsync("http://localhost:12701/api/locations/get?latitude=50&longitude=5&maxResults=-10&maxDistance=130000");

                    Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
                }
            }
        }



    }
}
