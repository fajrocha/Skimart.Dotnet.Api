using Application.Cases.Shared.Dtos;
using Domain.Entities.Product;
using MediatR;

namespace Application.Cases.Products.Queries.GetAllProductBrands;

public record GetAllBrandsQuery(HttpRequestDto RequestDto) : IRequest<IReadOnlyList<ProductBrand>>;