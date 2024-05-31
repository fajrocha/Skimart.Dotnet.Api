using Skimart.Application.Products.Queries.GetAllProducts;
using Skimart.Domain.Entities.Products;
using Skimart.Infrastructure.Products.Specifications.Extensions;
using Skimart.Infrastructure.Shared.Specifications;

namespace Skimart.Infrastructure.Products.Specifications;

public class ProductsWithFiltersCountSpec : BaseSpecification<Product>
{
    public ProductsWithFiltersCountSpec(GetAllProductsQuery productRequest) 
        : base(productRequest.SearchOrQueryFilters())
    {
    }
}