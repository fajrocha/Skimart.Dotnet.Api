namespace Skimart.Application.Gateways.Memory.Cache;

public interface ICacheService
{
    Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);
    Task<string?> GetCachedResponseAsync(string cacheKey);
}