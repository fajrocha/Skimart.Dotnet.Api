using Skimart.Application.Cache;
using Skimart.Application.Shared;
using Skimart.Domain.Entities.Products;

namespace Skimart.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery(int Id) : IResultRequest<Product>, ICacheRequest
{
    public string CacheKey => $"{nameof(GetProductByIdQuery)}-Request-{DateTime.UtcNow:yyyyMMdd}";
};
