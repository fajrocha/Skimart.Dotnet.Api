using Microsoft.Extensions.Caching.Distributed;
using Skimart.Application.Extensions.Serialization;
using Skimart.Application.Gateways.Memory.Cache;

namespace Skimart.Infrastructure.Cache;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;

    public RedisCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }
    
    public async Task SetCacheValueAsync<T>(string cacheKey, T? value, TimeSpan timeToLive) 
        where T : class
    {
        if (value is null) return;

        var serializedResponse = value.SerializeCamelCase();

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = timeToLive,
        };

        await _distributedCache.SetStringAsync(cacheKey, serializedResponse, cacheOptions);
    }

    public async Task<T?> GetOrCacheValueAsync<T>(string cacheKey, Func<Task<T?>> factory, TimeSpan timeToLive)
        where T : class
    {
        var cachedValue = await GetCachedValueAsync<T>(cacheKey);

        if (cachedValue is not null) 
            return cachedValue;

        cachedValue = await factory();
        await SetCacheValueAsync(cacheKey, cachedValue, timeToLive);

        return cachedValue;
    }

    public async Task<T?> GetCachedValueAsync<T>(string cacheKey) where T : class
    {
        var cacheValue =  await _distributedCache.GetStringAsync(cacheKey);

        return cacheValue?.DeserializeCamelCase<T>();
    }
}