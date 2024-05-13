using MediatR;
using Skimart.Application.Cache;

namespace Skimart.Application.Products.Queries.GetAllProducts;

public record GetAllProductsQuery(
    int PageIndex,
    int PageSize,
    int? BrandId,
    int? TypeId,
    string? Sort,
    string Search) : IRequest<ProductsResponseDto>
{
    public override string ToString()
    {
        return $"{nameof(PageIndex)}:{PageIndex}," +
               $"{nameof(PageSize)}:{PageSize}," +
               $"{nameof(BrandId)}:{BrandId}," +
               $"{nameof(TypeId)}:{TypeId}," +
               $"{nameof(Sort)}:{Sort}," +
               $"{nameof(Search)}:{Search}";
    }
}
