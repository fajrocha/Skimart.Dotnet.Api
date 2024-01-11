using Application.Cases.Shared.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Responses;
using Presentation.Responses.ErrorResponses;
using Skimart.Application.Cases.Products.Dtos;
using Skimart.Application.Cases.Products.Queries.GetAllProductBrands;
using Skimart.Application.Cases.Products.Queries.GetAllProducts;
using Skimart.Application.Cases.Products.Queries.GetAllProductTypes;
using Skimart.Application.Cases.Products.Queries.GetProductById;
using Skimart.Application.Extensions.FluentResults;
using Skimart.Responses;

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
    public async Task<IActionResult> GetProducts([FromQuery] ProductParams productParams)
    {
        var requestDto = GetRequestDto(HttpContext.Request);
        var query = new GetAllProductsQuery(productParams, requestDto);
        
        var result = await _mediator.Send(query);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(int id)
    {
        var requestDto = GetRequestDto(HttpContext.Request);
        var result = await _mediator.Send(new GetProductByIdQuery(id, requestDto));

        if (result.IsSuccess) 
            return Ok(result.Value);
        
        return NotFound(ErrorResponse.NotFound(result.GetReasonsAsCollection(), ErrorMessage));
    }
    
    [HttpGet("brands")]
    public async Task<IActionResult> GetProductsBrands()
    {
        var requestDto = GetRequestDto(HttpContext.Request);
        var query = new GetAllBrandsQuery(requestDto);
        var result = await _mediator.Send(query);
            
        return Ok(result);
    }

    // GET: api/Products/types
    [HttpGet("types")]
    public async Task<IActionResult> GetProductsTypes()
    {
        var requestDto = GetRequestDto(HttpContext.Request);
        var query = new GetAllTypesQuery(requestDto);
        var result = await _mediator.Send(query);
        
        return Ok(result);
    }

    private static HttpRequestDto GetRequestDto(HttpRequest httpRequest)
    {
        return new HttpRequestDto(httpRequest.Path, httpRequest.Query);
    }
}