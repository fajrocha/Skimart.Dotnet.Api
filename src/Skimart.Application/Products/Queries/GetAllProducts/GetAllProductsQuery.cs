using MediatR;
using Skimart.Application.Cache;

namespace Skimart.Application.Products.Queries.GetAllProducts;

public record GetAllProductsQuery(
    int PageIndex,
    int PageSize,
    int? BrandId,
    int? TypeId,
    string? Sort,
    string Search) : IRequest<ProductsResponseDto>, ICacheRequest
{
    public string CacheKey => $"{nameof(GetAllProductsQuery)}-Request-{DateTime.UtcNow:yyyyMMdd}";
}
