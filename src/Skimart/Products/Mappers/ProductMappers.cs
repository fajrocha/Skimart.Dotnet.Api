using Skimart.Application.Products.Queries.GetAllProducts;
using Skimart.Contracts.Products.Requests;
using Skimart.Contracts.Products.Responses;
using Skimart.Domain.Entities.Products;

namespace Skimart.Mappers.Products;

public static class ProductMappers
{
    private static string ApiUrl => Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Split(';').FirstOrDefault() 
                                    ?? string.Empty;
    
    public static ProductResponse ToResponse(this Product product)
    {
        return new ProductResponse(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            $"{ApiUrl}/{product.PictureUrl}",
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