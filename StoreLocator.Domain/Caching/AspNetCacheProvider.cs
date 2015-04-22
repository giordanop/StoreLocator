using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using StoreLocator.Domain.Setting;

namespace StoreLocator.Domain.Caching
{
    public class AspNetCacheProvider : ICacheProvider
    {
        private readonly Cache _cache;
        private readonly bool _enableCaching;
        private readonly TimeSpan _defaultExpirationWindow;

        public AspNetCacheProvider() : this (new DomainSettingsDefault()) { }

        public AspNetCacheProvider(IDomainSettings domainSettings)
        {
            if (HttpContext.Current != null)
                _cache = HttpContext.Current.Cache;
            else
                _cache = HttpRuntime.Cache;

            _enableCaching = domainSettings.EnableCaching;
            _defaultExpirationWindow = new TimeSpan(0, domainSettings.DefaultCacheExpirationInMin, 0);
        }

        public void Add(string cacheKey, object dataToAdd)
        {
            _cache.Add(cacheKey, dataToAdd, null, Cache.NoAbsoluteExpiration,
                    _defaultExpirationWindow, CacheItemPriority.BelowNormal, null);
        }

        public T Get<T>(string key) where T : class
        {
            try
            {
                return _enableCaching ? (T) _cache.Get(key) : null;
            }
            catch (Exception e)
            {
                //logging
                return null;
            }
        }

        public T TryGet<T>(string key, Func<T> fallback) where T : class
        {

            var results = Get<T>(key);

            try
            {
                if (results != null)
                    return results;

                results = fallback();

                return results;
            }
            catch (Exception e)
            {
                //logging
                throw;
            }
        }

        public void InvalidateCacheItem(string key)
        {
            if (_enableCaching && _cache.Get(key) != null)
                _cache.Remove(key);
        }
    }
}
