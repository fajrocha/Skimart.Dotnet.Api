using Application.Cases.Shared.Dtos;
using Domain.Entities.Product;
using MediatR;

namespace Application.Cases.Products.Queries.GetAllProductTypes;

public record GetAllTypesQuery(HttpRequestDto RequestDto) : IRequest<IReadOnlyList<ProductType>>;