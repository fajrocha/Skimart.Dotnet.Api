using Domain.Entities.Product;
using MediatR;
using Skimart.Application.Cases.Shared.Dtos;

namespace Skimart.Application.Cases.Products.Queries.GetAllProductBrands;

public record GetAllBrandsQuery(HttpRequestDto RequestDto) : IRequest<IReadOnlyList<ProductBrand>>;