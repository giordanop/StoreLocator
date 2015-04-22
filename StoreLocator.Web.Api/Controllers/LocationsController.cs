using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using StoreLocator.Domain.Dto;
using StoreLocator.Domain.Repository;
using StoreLocator.Web.Api.Extensions;
using StoreLocator.Web.Api.Models;

namespace StoreLocator.Web.Api.Controllers
{
    public class LocationsController : ApiController
    {
        private ILocationRepository _locationRepository;
        private ModelFactory _modelFactory;
        public LocationsController(ILocationRepository locationRepository, ModelFactory modelFactory)
        {
            _locationRepository = locationRepository;
            _modelFactory = modelFactory;
        }

        // GET api/values
        [CheckModelForNull]
        public HttpResponseMessage Get(double? latitude, double? longitude, int? maxResults, int? maxDistance)
        {
            //I've already checked if these value are null with my custome attribute CheckModelForNull

            var locationDto = new LocationDto { Latitude = latitude.Value, Longitude = longitude.Value };
            
            //checks

            //if (maxDistance.Value.Equals(0))  //Are there shops in the same buildings?

            if (maxResults.Value.Equals(0))
            {
                return Request.CreateResponse(HttpStatusCode.OK, _modelFactory.Create(new List<LocationDto>(), locationDto, maxDistance.Value, maxResults.Value));
            }

            if (maxDistance.Value < 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,"Max distance cannot be negative");
            }

            if (maxResults.Value < 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Max results cannot be negative");
            }

            //end checks

            var locations = _locationRepository.GetLocations(locationDto, maxDistance.Value, maxResults.Value);

            return Request.CreateResponse(HttpStatusCode.OK, _modelFactory.Create(locations, locationDto, maxDistance.Value, maxResults.Value));
            
        }
    }
}
