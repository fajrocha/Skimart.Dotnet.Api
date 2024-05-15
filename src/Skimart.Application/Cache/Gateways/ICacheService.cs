namespace Skimart.Application.Cache.Gateways;

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