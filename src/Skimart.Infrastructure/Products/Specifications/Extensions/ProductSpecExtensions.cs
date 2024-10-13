using System.Linq.Expressions;
using Skimart.Application.Products.Queries.GetAllProducts;
using Skimart.Domain.Entities.Products;

namespace Skimart.Infrastructure.Products.Specifications.Extensions;

public static class ProductSpecExtensions
{
    public static Expression<Func<Product, bool>> SearchOrQueryFilters(this GetAllProductsQuery productRequest)
    {
        return p =>
            (string.IsNullOrEmpty(productRequest.Search) || p.Name.ToLower().Contains(productRequest.Search)) &&
            (!productRequest.BrandId.HasValue || p.ProductBrandId == productRequest.BrandId) &&
            (!productRequest.TypeId.HasValue || p.ProductTypeId == productRequest.TypeId);
    }
}