using Application.Cases.Products.Dtos;
using Application.Cases.Products.Queries.GetAllProducts;
using Application.Cases.Products.Queries.GetProductById;
using Application.Cases.Shared.Dtos;
using Application.Extensions.FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Responses;
using Presentation.Responses.ErrorResponses;

namespace Presentation.Controllers;

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
        var requestDto = new HttpRequestDto(HttpContext.Request.Path, HttpContext.Request.Query);
        var query = new GetAllProductsQuery(productParams, requestDto);
        
        var result = await _mediator.Send(query);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(int id)
    {
        var requestDto = new HttpRequestDto(HttpContext.Request.Path, HttpContext.Request.Query);
        var result = await _mediator.Send(new GetProductByIdQuery(id, requestDto));

        if (result.IsSuccess) 
            return Ok(result.Value);
        
        return NotFound(ErrorResponse.NotFound(result.GetReasonsAsCollection(), ErrorMessage));
    }
}