using Domain.Entities.Product;
using Skimart.Application.Cases.Products.Queries.GetAllProducts;
using Skimart.Infrastructure.Persistence.Specifications.ProductSpecifications.Extensions;

namespace Skimart.Infrastructure.Persistence.Specifications.ProductSpecifications;

public class ProductsWithTypesAndBrandsSpec : BaseSpecification<Product>
{
    public ProductsWithTypesAndBrandsSpec(ProductParams productParams) : base(productParams.SearchOrQueryFilters())
    {
        AddInclude(p => p.ProductType);
        AddInclude(p => p.ProductBrand);
        AddOrderBy(p => p.Name);
        ApplyPagination(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);
        var sortingStrategy = productParams.Sort;

        if (string.IsNullOrEmpty(sortingStrategy)) 
            return;

        SelectSortingStrategy(sortingStrategy);
    }

    private void SelectSortingStrategy(string sortingStrategy)
    {
        switch (sortingStrategy)
        {
            case "priceAsc":
                AddOrderBy(p => p.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(p => p.Price);
                break;
            default:
                AddOrderBy(p => p.Name);
                break;
        }
    }
}