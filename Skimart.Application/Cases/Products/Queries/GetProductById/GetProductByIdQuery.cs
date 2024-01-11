using Application.Cases.Shared.Dtos;
using FluentResults;
using MediatR;
using Skimart.Application.Cases.Products.Dtos;

namespace Skimart.Application.Cases.Products.Queries.GetProductById;

public record GetProductByIdQuery(int Id, HttpRequestDto Request) : IRequest<Result<ProductToReturnDto>>
{
    public void Deconstruct(out int id, out HttpRequestDto request)
    {
        id = Id;
        request = Request;
    }
};