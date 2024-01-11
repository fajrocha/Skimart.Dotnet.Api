namespace Skimart.Application.Abstractions.Memory.Cache;

public interface ICacheService
{
    Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);
    Task<string?> GetCachedResponseAsync(string cacheKey);
}