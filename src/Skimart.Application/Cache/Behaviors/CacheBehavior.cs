using ErrorOr;
using MediatR;
using Skimart.Application.Extensions.Serialization;
using Skimart.Application.Gateways.Memory.Cache;
using Skimart.Domain.Entities.Products;

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

        var cachedData = await _cacheService.GetCachedResponseAsync(cacheKey);

        if (cachedData is not null) 
            return cachedData.DeserializeCamelCase<TResponse>();
        
        var response = await next();
        
        if (response is IErrorOr)
        {
            var value = response.ToErrorOr().Value as TResponse;
            await _cacheService.CacheResponseAsync(cacheKey, null, TimeSpan.FromSeconds(60));
        }
        else
        {
            await _cacheService.CacheResponseAsync(cacheKey, response.ToErrorOr(), TimeSpan.FromSeconds(60));
        }

        return response;
    }
}