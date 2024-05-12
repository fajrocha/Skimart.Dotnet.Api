namespace Skimart.Application.Gateways.Memory.Cache;

public interface ICacheService
{
    Task CacheValueAsync(string cacheKey, object response, TimeSpan timeToLive);
    Task<string?> GetCachedValueAsync(string cacheKey);
}