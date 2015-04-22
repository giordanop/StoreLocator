using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreLocator.Domain.Dto;

namespace StoreLocator.Domain.Repository
{
    public interface ILocationRepository
    {
        List<LocationDto> GetLocations(LocationDto location, int maxDistance, int maxResults);
    }
}
