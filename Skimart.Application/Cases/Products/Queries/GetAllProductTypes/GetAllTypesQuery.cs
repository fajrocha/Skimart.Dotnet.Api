using Domain.Entities.Product;
using MediatR;
using Skimart.Application.Cases.Shared.Dtos;

namespace Skimart.Application.Cases.Products.Queries.GetAllProductTypes;

public record GetAllTypesQuery(HttpRequestDto RequestDto) : IRequest<IReadOnlyList<ProductType>>;