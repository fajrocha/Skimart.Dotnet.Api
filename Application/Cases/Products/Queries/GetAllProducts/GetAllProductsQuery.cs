using Application.Cases.Products.Dtos;
using Application.Cases.Shared.Dtos;
using Application.Cases.Shared.Vms;
using MediatR;

namespace Application.Cases.Products.Queries.GetAllProducts;

public record GetAllProductsQuery(ProductParams ProductParams, HttpRequestDto Request)
    : IRequest<PaginatedDataVm<ProductToReturnDto>>
{
    public void Deconstruct(out ProductParams productParams, out HttpRequestDto request)
    {
        productParams = ProductParams;
        request = Request;
    }
};