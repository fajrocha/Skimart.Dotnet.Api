﻿using Application.Cases.Shared.Dtos;

namespace Skimart.Application.Abstractions.Memory.Cache;

public interface ICacheHandler
{
    Task<T?> GetCachedResponseAsync<T>(HttpRequestDto requestDto) where T : class;

    Task CacheResponseAsync(object response, TimeSpan timeToLive);
}