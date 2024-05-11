using MediatR;
using Skimart.Domain.Entities.Products;

namespace Skimart.Application.Products.Queries.GetAllProductTypes;

public record GetAllTypesQuery() : IRequest<List<ProductType>>;