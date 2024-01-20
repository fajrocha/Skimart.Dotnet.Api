using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skimart.Application.Cases.Auth.Commands;
using Skimart.Application.Cases.Auth.Commands.Login;
using Skimart.Application.Cases.Auth.Commands.Register;
using Skimart.Application.Cases.Auth.Dtos;
using Skimart.Application.Cases.Auth.Queries.GetCurrentLoggedUser;
using Skimart.Application.Extensions.FluentResults;
using Skimart.Extensions.FluentResults;
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
    
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var query = new GetCurrentLoggedUserQuery(User);
        var result = await _mediator.Send(query);
        
        return result.ToOkOrNotFound(ErrorMessage);
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> Login(LoginCommand loginCommand)
    {
        var result = await _mediator.Send(loginCommand);
        
        return result.ToOkOrUnauthorized(ErrorMessage);
    }
    
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> Register(RegisterCommand registerCommand)
    {
        var result = await _mediator.Send(registerCommand);
        
        return result.ToOkOrBadRequest(ErrorMessage);
    }
}