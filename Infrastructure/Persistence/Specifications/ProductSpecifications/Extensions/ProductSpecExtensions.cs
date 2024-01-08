using System.Linq.Expressions;
using Application.Cases.Products.Queries;
using Application.Cases.Products.Queries.GetAllProducts;
using Domain.Entities.Product;

namespace Infrastructure.Persistence.Specifications.ProductSpecifications.Extensions;

public static class ProductSpecExtensions
{
    public static Expression<Func<Product, bool>> SearchOrQueryFilters(this ProductParams productParams)
    {
        return p =>
            (string.IsNullOrEmpty(productParams.Search) || p.Name.ToLower().Contains(productParams.Search)) &&
            (!productParams.BrandId.HasValue || p.ProductBrandId == productParams.BrandId) &&
            (!productParams.TypeId.HasValue || p.ProductTypeId == productParams.TypeId);
    }
}