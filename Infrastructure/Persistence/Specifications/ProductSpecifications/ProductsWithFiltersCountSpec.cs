using Application.Cases.Products.Queries.GetAllProducts;
using Domain.Entities.Product;
using Infrastructure.Persistence.Specifications.ProductSpecifications.Extensions;

namespace Infrastructure.Persistence.Specifications.ProductSpecifications;

public class ProductsWithFiltersCountSpec : BaseSpecification<Product>
{
    public ProductsWithFiltersCountSpec(ProductParams productParams) 
        : base(productParams.SearchOrQueryFilters())
    {
    }
}