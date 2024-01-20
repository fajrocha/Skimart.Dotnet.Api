using FluentResults;
using MediatR;
using Skimart.Application.Cases.Products.Dtos;
using Skimart.Application.Cases.Shared.Dtos;

namespace Skimart.Application.Cases.Products.Queries.GetProductById;

public record GetProductByIdQuery(int Id, HttpRequestDto Request) : IRequest<Result<ProductDto>>
{
    public void Deconstruct(out int id, out HttpRequestDto request)
    {
        id = Id;
        request = Request;
    }
};