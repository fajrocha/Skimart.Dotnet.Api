using Skimart.Application.Products.Queries.GetAllProducts;
using Skimart.Domain.Entities.Products;
using Skimart.Infrastructure.Persistence.Specifications.ProductSpecifications.Extensions;

namespace Skimart.Infrastructure.Persistence.Specifications.ProductSpecifications;

public class ProductsWithFiltersCountSpec : BaseSpecification<Product>
{
    public ProductsWithFiltersCountSpec(GetAllProductsQuery productRequest) 
        : base(productRequest.SearchOrQueryFilters())
    {
    }
}