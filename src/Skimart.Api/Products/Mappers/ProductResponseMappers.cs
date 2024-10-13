using Skimart.Contracts.Products.Responses;
using Skimart.Domain.Entities.Products;
using Skimart.Shared.Extensions;

namespace Skimart.Products.Mappers;

public static class ProductResponseMappers
{
    public static ProductResponse ToResponse(this Product product)
    {
        return new ProductResponse(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.PictureUrl.CombineWithApiUrl(),
            product.ProductType.Name,
            product.ProductBrand.Name);
    }
    
    public static ProductTypeResponse ToResponse(this ProductType productType)
    {
        return new ProductTypeResponse(productType.Id, productType.Name);
    }
    
    public static ProductBrandResponse ToResponse(this ProductBrand productBrand)
    {
        return new ProductBrandResponse(productBrand.Id, productBrand.Name);
    }
}