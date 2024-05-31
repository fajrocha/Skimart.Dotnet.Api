using Skimart.Infrastructure.Cache.Configurations;
using Skimart.Infrastructure.Cache.Helpers;

namespace Skimart.Infrastructure.Cache.Abstractions;

public interface ICacheService
{
    Task SetCacheValueAsync<T>(string cacheKey, T? value, TimeSpan timeToLive) where T : class;
    Task<T?> GetOrCacheValueAsync<T>(string cacheKey, Func<Task<T?>> factory, TimeSpan timeToLive) where T : class;
    Task<T?> GetOrCacheValueAsync<T>(
        Action<CacheKeyBuilder> cacheKeyFactory, 
        Func<Task<T?>> valueFactory, 
        TimeSpan timeToLive) where T : class;
    Task<T?> GetCachedValueAsync<T>(string cacheKey) where T : class;
}