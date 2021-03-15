using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TSoft.Framework.CacheMemory.Implementations;
using TSoft.Framework.CacheMemory.Interfaces;

namespace Tsoft.Framework.CacheMemory
{
    public static class CacheMemoryCollectionExtension
    {
        public static IServiceCollection RegisterCacheMemory(this IServiceCollection services, IConfiguration configuration)
        {
            var cacheConfig = configuration.GetSection("CacheConfig");
            if (cacheConfig == null)
            {
                throw new System.Exception("Thêm CacheConfig vào appsettings.json");
            }
            services.AddSingleton<ICacheBase, CacheMemoryHelper>();
            services.AddSingleton<ICacheMemoryConfiguration>(configuration.GetSection("CacheConfig").Get<CacheMemoryConfiguration>());
            return services;
        }
    }
}
