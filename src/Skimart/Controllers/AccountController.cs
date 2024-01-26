using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skimart.Application.Cases.Auth.Commands;
using Skimart.Application.Cases.Auth.Commands.Login;
using Skimart.Application.Cases.Auth.Commands.Register;
using Skimart.Application.Cases.Auth.Commands.UpdateAddress;
using Skimart.Application.Cases.Auth.Dtos;
using Skimart.Application.Cases.Auth.Queries.CheckExistingEmail;
using Skimart.Application.Cases.Auth.Queries.GetCurrentLoggedUser;
using Skimart.Application.Cases.Auth.Queries.GetUserAddress;
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
    
    [HttpGet("email")]
    [AllowAnonymous]
    public async Task<bool> CheckEmailExists([FromQuery] string email)
    {
        var query = new CheckExistingEmailQuery(email);
        
        return await _mediator.Send(query);
    }
    
    [HttpGet("address")]
    public async Task<ActionResult<AddressDto>> GetAddress()
    {
        var query = new GetUserAddressQuery(User);
        var result = await _mediator.Send(query);
        
        return result.ToOkOrNotFound(ErrorMessage) ;
    }
    
    [HttpPut("address")]
    public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto addressDto)
    {
        var query = new UpdateAddressCommand(addressDto, User);
        var result = await _mediator.Send(query);
        
        return result.ToOkOrBadRequest(ErrorMessage) ;
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