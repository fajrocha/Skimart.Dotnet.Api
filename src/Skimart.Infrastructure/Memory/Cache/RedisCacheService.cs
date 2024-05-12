using Skimart.Application.Extensions.Serialization;
using Skimart.Application.Gateways.Memory.Cache;
using Skimart.Application.Helpers;
using StackExchange.Redis;

namespace Skimart.Infrastructure.Memory.Cache;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _database;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }
    
    public async Task CacheValueAsync(string cacheKey, object? response, TimeSpan timeToLive)
    {
        if (response is null) return;

        var serializedResponse = response.SerializeCamelCase();

        await _database.StringSetAsync(cacheKey, serializedResponse, timeToLive);
    }

    public async Task<string?> GetCachedValueAsync(string cacheKey)
    {
        return await _database.StringGetAsync(cacheKey);
    }
}