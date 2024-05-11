using MediatR;
using Skimart.Domain.Entities.Products;

namespace Skimart.Application.Products.Queries.GetAllProductBrands;

public record GetAllBrandsQuery() : IRequest<List<ProductBrand>>;