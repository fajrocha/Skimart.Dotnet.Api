using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skimart.Application.Products.Queries.GetAllProductBrands;
using Skimart.Application.Products.Queries.GetAllProductTypes;
using Skimart.Application.Products.Queries.GetProductById;
using Skimart.Contracts.Products.Requests;
using Skimart.Contracts.Products.Responses;
using Skimart.Contracts.Shared;
using Skimart.Products.Mappers;
using Skimart.Responses;

namespace Skimart.Controllers;

[AllowAnonymous]
public class ProductsController : BaseController
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] ProductRequest productsRequest)
    {
        var query = productsRequest.ToQuery();

        var products = await _mediator.Send(query);
        
        return Ok(
            new PaginatedDataResponse<ProductResponse>(
                productsRequest.PageIndex,
                productsRequest.PageSize,
                products.ProductCount,
                products.Products.ToList().ConvertAll(product => product.ToResponse())));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var query = new GetProductByIdQuery(id);
        var result = await _mediator.Send(query);
        
        return result.Match(
            product => Ok(product.ToResponse()),
            Problem);
    }
    
    [HttpGet("brands")]
    public async Task<IActionResult> GetProductsBrands()
    {
        var query = new GetAllBrandsQuery();

        var productBrands = await _mediator.Send(query);
        
        return Ok(productBrands.ConvertAll(productBrand => productBrand.ToResponse()));
    }

    // GET: api/Products/types
    [HttpGet("types")]
    public async Task<IActionResult> GetProductsTypes()
    {
        var query = new GetAllTypesQuery();

        var productTypes = await _mediator.Send(query);
        
        return Ok(productTypes.ConvertAll(productType => productType.ToResponse()));;
    }
}