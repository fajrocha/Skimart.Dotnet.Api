using System.Text;
using Skimart.Application.Abstractions.Memory.Cache;
using Skimart.Application.Cases.Shared.Dtos;

namespace Skimart.Application.Helpers;

public class CacheHandler : ICacheHandler
{
    private readonly ICacheService _cacheService;
    private string _cacheKey = string.Empty;

    public CacheHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }
    
    public async Task<T?> GetCachedResponseAsync<T>(HttpRequestDto requestDto) where T : class
    {
        _cacheKey = GetKeyFromRequest(requestDto);
        var cachedResponse = await _cacheService.GetCachedResponseAsync(_cacheKey);
        
        return !string.IsNullOrEmpty(cachedResponse)
            ? SystemJsonSerializer.DeserializeCamelCase<T>(cachedResponse)
            : null;
    }
    
    public async Task CacheResponseAsync(object response, TimeSpan timeToLive)
    {
        if (string.IsNullOrEmpty(_cacheKey)) 
            return;
        
        await _cacheService.CacheResponseAsync(_cacheKey, response, timeToLive);
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