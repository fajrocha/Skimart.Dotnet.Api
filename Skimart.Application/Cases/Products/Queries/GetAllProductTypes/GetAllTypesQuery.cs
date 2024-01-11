using Application.Cases.Shared.Dtos;
using Domain.Entities.Product;
using MediatR;

namespace Skimart.Application.Cases.Products.Queries.GetAllProductTypes;

public record GetAllTypesQuery(HttpRequestDto RequestDto) : IRequest<IReadOnlyList<ProductType>>;