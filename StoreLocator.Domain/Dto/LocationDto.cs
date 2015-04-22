using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreLocator.Domain.Dto
{
    public class LocationDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override bool Equals(object obj)
        {
            var location = (LocationDto) obj;
            return Latitude.Equals(location.Latitude) && Longitude.Equals(location.Longitude) ;
        }

        public override int GetHashCode()
        {
            return Latitude.GetHashCode() + Longitude.GetHashCode();
        }
    }
}
