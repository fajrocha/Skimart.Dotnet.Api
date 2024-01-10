using Application.Cases.Products.Dtos;
using Application.Cases.Shared.Dtos;
using FluentResults;
using MediatR;

namespace Application.Cases.Products.Queries.GetProductById;

public record GetProductByIdQuery(int Id, HttpRequestDto Request) : IRequest<Result<ProductToReturnDto>>
{
    public void Deconstruct(out int id, out HttpRequestDto request)
    {
        id = Id;
        request = Request;
    }
};