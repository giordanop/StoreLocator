using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using StoreLocator.Domain.Caching;
using StoreLocator.Domain.Dto;
using StoreLocator.Domain.Entities;
using StoreLocator.Domain.Repository;
using StoreLocator.Domain.Setting;

namespace StoreLocator.Domain.Tests.Repository
{
    [TestFixture]
    public class LocationRepositoryIntegrationTests
    {

        [SetUp]
        public void Initialize()
        {

        }

        [Test]
        public void GetLocation_with_real_params_should_return_some_results()
        {
            var location = new LocationDto() {Latitude = 50d, Longitude = 5d};
            const int maxResults = 1000;
            const int maxDistance = 3000000;

            var repository = new LocationRepository(new StoreLocatorEntities(), new AspNetCacheProvider());

            var result = repository.GetLocations(location, maxDistance, maxResults);

            Assert.IsTrue(result.Any());
        }


        [Test]
        public void
            GetLocation_test_with_various_maxDistances_should_be_get_the_same_results_of_the_aspnet_code_function()
        {

            var location = new LocationDto() {Latitude = 50d, Longitude = 5d};
            const int maxResults = 10000000;
            var maxDistances = new[] {10000, 50000};

            var allLocations = new StoreLocatorEntities().Location.ToList();
            foreach (var maxDistance in maxDistances)
            {
                var distance = maxDistance;
                var resultCodeFunction =
                    allLocations.Where(
                        i =>
                            IsInRange(location, new LocationDto() {Latitude = i.Latitude, Longitude = i.Longitude},
                                distance))
                        .Select(i => new LocationDto() {Latitude = i.Latitude, Longitude = i.Longitude})
                        .Take(maxResults);

                var repository = new LocationRepository(new StoreLocatorEntities(), new AspNetCacheProvider());
                var locations = repository.GetLocations(location, maxDistance, maxResults);


                CollectionAssert.AreEquivalent(resultCodeFunction, locations);
            }
        }


        [Test]
        public void GetLocation_use_cache()
        {

            var location = new LocationDto() {Latitude = 50d, Longitude = 5d};
            const int maxResults = 100000000;
            var maxDistance = 3000000;

            var repository = new LocationRepository(new StoreLocatorEntities(),
                new AspNetCacheProvider(new DomainSettingsCacheEnabled()));
            var watch = Stopwatch.StartNew();
            var locations = repository.GetLocations(location, maxDistance, maxResults);

            var foundLocations = locations.ToList();

            Debug.WriteLine(watch.Elapsed);
            Debug.WriteLine("Results found: " + locations.Count());
            Debug.WriteLine("Total rows: " + new StoreLocatorEntities().Location.Count());

            var watch2 = Stopwatch.StartNew();
            var locations2 = repository.GetLocations(location, maxDistance, maxResults);

            var foundLocations2 = locations2.ToList();

            Debug.WriteLine(watch2.Elapsed);
            Debug.WriteLine("Results found: " + locations2.Count());
            Debug.WriteLine("Total rows: " + new StoreLocatorEntities().Location.Count());

        }





        private Boolean IsInRange(LocationDto location1, LocationDto location2, int maxDistance)
        {
            var rlat1 = Math.PI*location1.Latitude/180;
            var rlat2 = Math.PI*location2.Latitude/180;
            var theta = location1.Longitude - location2.Longitude;
            var rtheta = Math.PI*theta/180;
            var dist = Math.Sin(rlat1)*Math.Sin(rlat2) + Math.Cos(rlat1)*Math.Cos(rlat2)*Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist*180/Math.PI;
            dist = dist*60*1.1515;
            dist = dist*1609.344;
            return dist < maxDistance;
        }
    }
}
