using System;
using System.IO;
using ItSynced.Web.DAL.LocalsystemCrawler;
using Microsoft.Framework.Caching.Memory;

namespace ItSynced.Web.DAL.MemoryCache
{
    public class CacheRepository
    {
        private readonly Microsoft.Framework.Caching.Memory.MemoryCache _cache;

        public CacheRepository()
        {
            _cache = new Microsoft.Framework.Caching.Memory.MemoryCache(new MemoryCacheOptions());
        }

        public object GetItem(string directory)
        {
            object cachedObject;
            if (_cache.TryGetValue(directory, out cachedObject))
            {
                return cachedObject;
            }

            _cache.Set(directory, context =>
            {
                context.SetAbsoluteExpiration(TimeSpan.FromSeconds(10));
                return new Lazy<object>(() => InitItem(directory)).Value;
            });

            return GetItem(directory);
        }

        private object InitItem(string directory)
        {
            return new DirectoryCrawler().GetDirectories(directory);
        }
    }
}