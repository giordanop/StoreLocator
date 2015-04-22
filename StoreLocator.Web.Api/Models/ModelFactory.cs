using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoreLocator.Domain.Dto;

namespace StoreLocator.Web.Api.Models
{
    public class ModelFactory
    {
        internal SearchResultModel Create(List<LocationDto> locationsFound, LocationDto originalLocation, int maxDistance, int maxResults)
        {
            return new SearchResultModel
            {
                LocationsFound = locationsFound.ToArray(),
                MaxDistance = maxDistance,
                MaxResults = maxResults,
                NumResults = locationsFound.Count(),
                OriginalLocation = originalLocation
            };
        }
    }
}