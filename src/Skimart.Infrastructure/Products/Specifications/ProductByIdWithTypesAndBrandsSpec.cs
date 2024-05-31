using Skimart.Domain.Entities.Products;
using Skimart.Infrastructure.Shared.Specifications;

namespace Skimart.Infrastructure.Products.Specifications;

public class ProductByIdWithTypesAndBrandsSpec : BaseSpecification<Product>
{
    public ProductByIdWithTypesAndBrandsSpec(int id) : base(x => x.Id == id)
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
    }
}