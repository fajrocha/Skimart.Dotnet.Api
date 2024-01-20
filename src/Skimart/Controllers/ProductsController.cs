using Domain.Entities.Product;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skimart.Application.Cases.Products.Dtos;
using Skimart.Application.Cases.Products.Queries.GetAllProductBrands;
using Skimart.Application.Cases.Products.Queries.GetAllProducts;
using Skimart.Application.Cases.Products.Queries.GetAllProductTypes;
using Skimart.Application.Cases.Products.Queries.GetProductById;
using Skimart.Application.Cases.Shared.Dtos;
using Skimart.Application.Cases.Shared.Vms;
using Skimart.Application.Extensions.FluentResults;
using Skimart.Extensions.FluentResults;
using Skimart.Extensions.Request;
using Skimart.Responses;
using Skimart.Responses.ErrorResponses;

namespace Skimart.Controllers;

[AllowAnonymous]
public class ProductsController : BaseController
{
    private const string ErrorMessage = "Error on Product request.";
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<PaginatedDataVm<ProductDto>> GetProducts([FromQuery] ProductParams productParams)
    {
        var query = new GetAllProductsQuery(productParams, HttpContext.Request.ToDto());
        
        return await _mediator.Send(query);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetProductById(int id)
    {
        var query = new GetProductByIdQuery(id, HttpContext.Request.ToDto());
        var result = await _mediator.Send(query);

        return result.ToOkOrNotFound(ErrorMessage);
    }
    
    [HttpGet("brands")]
    public async Task<IReadOnlyList<ProductBrand>> GetProductsBrands()
    {
        var query = new GetAllBrandsQuery(HttpContext.Request.ToDto());
        
        return await _mediator.Send(query);
    }

    // GET: api/Products/types
    [HttpGet("types")]
    public async Task<IReadOnlyList<ProductType>> GetProductsTypes()
    {
        var query = new GetAllTypesQuery(HttpContext.Request.ToDto());
        
        return await _mediator.Send(query);
    }
}