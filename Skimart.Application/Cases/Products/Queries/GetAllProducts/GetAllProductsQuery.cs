using MediatR;
using Skimart.Application.Cases.Products.Dtos;
using Skimart.Application.Cases.Shared.Dtos;
using Skimart.Application.Cases.Shared.Vms;

namespace Skimart.Application.Cases.Products.Queries.GetAllProducts;

public record GetAllProductsQuery(ProductParams ProductParams, HttpRequestDto Request)
    : IRequest<PaginatedDataVm<ProductToReturnDto>>
{
    public void Deconstruct(out ProductParams productParams, out HttpRequestDto request)
    {
        productParams = ProductParams;
        request = Request;
    }
};