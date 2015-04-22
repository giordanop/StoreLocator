using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using StoreLocator.Domain.Caching;
using StoreLocator.Domain.Dto;
using StoreLocator.Domain.Entities;

namespace StoreLocator.Domain.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private const string LocationCacheKey = "Location";

        private StoreLocatorEntities _dbContext;
        private ICacheProvider _cacheProvider;
        public LocationRepository(StoreLocatorEntities dbContext, ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
            _dbContext = dbContext;
        }

        public List<LocationDto> GetLocations(LocationDto location, int maxDistance, int maxResults)
        {
            try
            {

                var cacheKey = BuildCacheKey(location, maxDistance, maxResults);

                return _cacheProvider.TryGet(cacheKey, () =>
                {
                    var locations = _dbContext.Location

                        //create new object complex: location+distance from original location
                        .Select(l => new
                        {
                            l,
                            dist = SqlFunctions.Acos( // Math.Acos(dist);
                                SqlFunctions.Sin(Math.PI*l.Latitude/180d)* //Math.Sin(Math.PI * Latitude / 180)
                                SqlFunctions.Sin(Math.PI*location.Latitude/180d) +  //Math.Sin(Math.PI * location.Latitude / 180)
                                SqlFunctions.Cos(Math.PI*l.Latitude/180d)* //Math.Cos(Math.PI * Latitude / 180)
                                SqlFunctions.Cos(Math.PI*location.Latitude/180d)* //Math.Cos(Math.PI * location.Latitude / 180)
                                SqlFunctions.Cos(Math.PI*(l.Longitude - location.Longitude)/180d)  //Math.Cos(Math.PI * (Longitude - location.Longitude) / 180)
                                )*180d/Math.PI*60d*1.1515d*1609.344d
                        })

                        //where distance is less than the given distance
                        .Where(locationObject => locationObject.dist < (double) maxDistance)

                        //order by distance
                        .OrderBy(locationObject => locationObject.dist)

                        //take maxresults
                        .Take(maxResults)

                        //wrap in LocationDto object
                        .Select(locationObject =>
                            new LocationDto()
                            {
                                Latitude = locationObject.l.Latitude,
                                Longitude = locationObject.l.Longitude
                            })

                        //do query!
                        .ToList();

                    _cacheProvider.Add(cacheKey, locations);

                    return locations;
                });

                
            }
            catch (Exception e)
            {
                //logging
                throw;
            }
        }

        /// <summary>
        /// Build a cache key for that request
        /// </summary>
        /// <param name="location"></param>
        /// <param name="maxDistance"></param>
        /// <param name="maxResults"></param>
        /// <returns></returns>
        private string BuildCacheKey(LocationDto location, int maxDistance, int maxResults)
        {
            return String.Format("{0}_{1}_{2}_{3}_{4}",LocationCacheKey,location.Latitude,location.Longitude,maxDistance,maxResults);
        }
    }
}
