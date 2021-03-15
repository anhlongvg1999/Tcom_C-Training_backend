using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSoft.Framework.CacheMemory.Interfaces;

namespace TSoft.Framework.CacheMemory.Implementations
{
    public class CacheMemoryHelper : ICacheBase
    {
        private readonly ICacheMemoryConfiguration _cacheMemoryConfiguration;
        private IMemoryCache _cache;

        public CacheMemoryHelper(IMemoryCache cache,
            ICacheMemoryConfiguration cacheMemoryConfiguration)
        {
            this._cache = cache;
            _cacheMemoryConfiguration = cacheMemoryConfiguration;
        }
        public async Task<T> GetOrCreate<T>(string key, Func<Task<T>> createEntry)
        {
            var data = _cache.Get<T>(key);
            if(data == null)
            {
                if (_cacheMemoryConfiguration.EnableCache == true)
                {
                    T cacheEntry;

                    // Look for cache key.
                    if (!_cache.TryGetValue(key, out cacheEntry))
                    {
                        // Key not in cache, so get data.
                        cacheEntry = await createEntry();

                        // Set cache options.
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            // Keep in cache for this time, reset time if accessed.
                            .SetSlidingExpiration(TimeSpan.FromSeconds(7200)); // 2h

                        // Save data in cache.
                        _cache.Set(key, cacheEntry, cacheEntryOptions);
                        data = cacheEntry;
                    }
                }
            }
            return data;
        }
        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
