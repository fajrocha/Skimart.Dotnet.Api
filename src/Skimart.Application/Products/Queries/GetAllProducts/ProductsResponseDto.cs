using Skimart.Domain.Entities.Products;

namespace Skimart.Application.Products.Queries.GetAllProducts;

public record ProductsResponseDto(int ProductCount, List<Product> Products);