using Application.Cases.Products.Queries.GetAllProducts;
using Application.Cases.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[AllowAnonymous]
public class ProductsController : BaseController
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] ProductParams productParams)
    {
        var requestDto = new HttpRequestDto(HttpContext.Request.Path, HttpContext.Request.Query);
        var query = new GetAllProductsQuery(productParams, requestDto);
        
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}