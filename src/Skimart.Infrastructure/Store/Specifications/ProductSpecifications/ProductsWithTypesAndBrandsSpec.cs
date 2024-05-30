using Skimart.Application.Products.Queries.GetAllProducts;
using Skimart.Domain.Entities.Products;
using Skimart.Infrastructure.Persistence.Specifications.ProductSpecifications.Extensions;

namespace Skimart.Infrastructure.Persistence.Specifications.ProductSpecifications;

public class ProductsWithTypesAndBrandsSpec : BaseSpecification<Product>
{
    public ProductsWithTypesAndBrandsSpec(GetAllProductsQuery productsQuery) : base(productsQuery.SearchOrQueryFilters())
    {
        AddInclude(p => p.ProductType);
        AddInclude(p => p.ProductBrand);
        AddOrderBy(p => p.Name);
        ApplyPagination(productsQuery.PageSize * (productsQuery.PageIndex - 1), productsQuery.PageSize);
        var sortingStrategy = productsQuery.Sort;

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