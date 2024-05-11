using MediatR;

namespace Skimart.Application.Products.Queries.GetAllProducts;

public record GetAllProductsQuery(
    int PageIndex,
    int PageSize,
    int? BrandId,
    int? TypeId,
    string? Sort,
    string Search) : IRequest<ProductsResponseDto>;
