using Domain.Entities.Product;

namespace Infrastructure.Persistence.Specifications.ProductSpecifications;

public class ProductByIdWithTypesAndBrandsSpec : BaseSpecification<Product>
{
    public ProductByIdWithTypesAndBrandsSpec(int id) : base(x => x.Id == id)
    {
        AddInclude(x => x.ProductType);
        AddInclude(x => x.ProductBrand);
    }
}