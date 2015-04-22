using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreLocator.Domain.Caching
{
    public interface ICacheProvider
    {
        void Add(string key, object value);
        T Get<T>(string key) where T : class;
        T TryGet<T>(string key, Func<T> fallback) where T : class;
        void InvalidateCacheItem(string key);
    }
}
