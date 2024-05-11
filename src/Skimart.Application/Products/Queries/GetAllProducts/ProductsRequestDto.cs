namespace Skimart.Application.Products.Queries.GetAllProducts;

public record ProductsRequestDto(
    int PageIndex,
    int PageSize,
    int? BrandId,
    int? TypeId,
    string? Sort,
    string Search);