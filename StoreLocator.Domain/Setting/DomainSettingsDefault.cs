using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreLocator.Domain.Setting
{
    public class DomainSettingsDefault : IDomainSettings
    {
        public bool EnableCaching
        {
            get { return false;}
        }

        public int DefaultCacheExpirationInMin
        {
            get { return 0; }
        }
    }
}
