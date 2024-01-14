using System.Text;
using Skimart.Application.Abstractions.Memory.Cache;
using Skimart.Application.Cases.Shared.Dtos;

namespace Skimart.Application.Helpers;

public class CacheHandler : ICacheHandler
{
    private readonly ICacheService _cacheService;

    public CacheHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }
    
    public async Task<T?> GetCachedResponseAsync<T>(HttpRequestDto requestDto) where T : class
    {
        var cacheKey = GetKeyFromRequest(requestDto);
        var cachedResponse = await _cacheService.GetCachedResponseAsync(cacheKey);
        
        return !string.IsNullOrEmpty(cachedResponse)
            ? SystemJsonSerializer.DeserializeCamelCase<T>(cachedResponse)
            : null;
    }
    
    public async Task CacheResponseAsync(HttpRequestDto requestDto, object response, TimeSpan timeToLive)
    {
        var cacheKey = GetKeyFromRequest(requestDto);
        
        await _cacheService.CacheResponseAsync(cacheKey, response, timeToLive);
    }
    
    private static string GetKeyFromRequest(HttpRequestDto request)
    {
        var keyBuilder = new StringBuilder();

        keyBuilder.Append($"{request.Path}");

        foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
        {
            keyBuilder.Append($"|{key}-{value}");
        }

        return keyBuilder.ToString();
    }
}