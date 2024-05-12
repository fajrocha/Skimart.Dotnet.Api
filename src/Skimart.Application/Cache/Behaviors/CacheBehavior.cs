using MediatR;
using Skimart.Application.Extensions.Serialization;
using Skimart.Application.Gateways.Memory.Cache;

namespace Skimart.Application.Cache.Behaviors;

public class CacheBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : ICacheRequest
{
    private readonly ICacheService _cacheService;

    public CacheBehavior(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var cacheKey = request.CacheKey;

        var cachedData = await _cacheService.GetCachedValueAsync(cacheKey);

        if (cachedData is not null)
        {
            var a = cachedData.DeserializeCamelCase<TResponse>();
            return a;
        }
        
        var response = await next();
        await _cacheService.CacheValueAsync(cacheKey, response!, TimeSpan.FromSeconds(60));

        return response;
    }
}