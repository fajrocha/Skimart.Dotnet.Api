using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skimart.Application.Cases.Auth.Commands;
using Skimart.Application.Cases.Auth.Dtos;
using Skimart.Application.Extensions.FluentResults;
using Skimart.Responses.ErrorResponses;

namespace Skimart.Controllers;

public class AccountController : BaseController
{
    private readonly IMediator _mediator;
    private const string ErrorMessage = "Error on Account request.";

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> Login(LoginCommand loginCommand)
    {
        var result = await _mediator.Send(loginCommand);
        
        return result.IsSuccess ? 
                Ok(result.Value) : 
                Unauthorized(ErrorResponse.Unauthorized(result.GetReasonsAsCollection(), ErrorMessage));
    }
}