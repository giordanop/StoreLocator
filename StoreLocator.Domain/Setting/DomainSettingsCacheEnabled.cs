using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreLocator.Domain.Setting
{
    public class DomainSettingsCacheEnabled : IDomainSettings
    {
        public DomainSettingsCacheEnabled()
        {
            
        }
        public bool EnableCaching
        {
            get { return true;}
        }

        public int DefaultCacheExpirationInMin
        {
            get { return 10; }
        }
    }
}
