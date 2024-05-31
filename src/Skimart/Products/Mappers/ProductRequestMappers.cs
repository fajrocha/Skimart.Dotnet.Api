using Skimart.Application.Products.Queries.GetAllProducts;
using Skimart.Contracts.Products.Requests;

namespace Skimart.Products.Mappers;

public static class ProductRequestMappers
{
    public static GetAllProductsQuery ToQuery(this ProductRequest productRequest)
    {
        return new GetAllProductsQuery(
            productRequest.PageIndex,
            productRequest.PageSize,
            productRequest.BrandId,
            productRequest.TypeId,
            productRequest.Sort,
            productRequest.Search);
    }
}