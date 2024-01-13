using Domain.Entities.Product;
using Skimart.Application.Cases.Products.Queries.GetAllProducts;
using Skimart.Infrastructure.Persistence.Specifications.ProductSpecifications.Extensions;

namespace Skimart.Infrastructure.Persistence.Specifications.ProductSpecifications;

public class ProductsWithFiltersCountSpec : BaseSpecification<Product>
{
    public ProductsWithFiltersCountSpec(ProductParams productParams) 
        : base(productParams.SearchOrQueryFilters())
    {
    }
}