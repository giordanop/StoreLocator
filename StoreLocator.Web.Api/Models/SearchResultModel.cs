using System.Collections.Generic;
using System.Linq;
using StoreLocator.Domain.Dto;

namespace StoreLocator.Web.Api.Models
{
    public class SearchResultModel
    {
        public LocationDto OriginalLocation { get; set; }
        public LocationDto[] LocationsFound { get; set; }
        public int NumResults { get; set; }
        public int MaxDistance { get; set; }
        public int MaxResults { get; set; }

    }
}
