using Application.Cases.Shared.Dtos;
using Domain.Entities.Product;
using MediatR;

namespace Skimart.Application.Cases.Products.Queries.GetAllProductBrands;

public record GetAllBrandsQuery(HttpRequestDto RequestDto) : IRequest<IReadOnlyList<ProductBrand>>;