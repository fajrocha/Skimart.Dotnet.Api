using Skimart.Application.Cases.Shared.Dtos;

namespace Skimart.Application.Abstractions.Memory.Cache;

public interface ICacheHandler
{
    Task<T?> GetCachedResponseAsync<T>(HttpRequestDto requestDto) where T : class;

    Task CacheResponseAsync(HttpRequestDto requestDto, object response, TimeSpan timeToLive);
}