using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skimart.Application.Cases.Auth.Commands.UpdateAddress;
using Skimart.Application.Cases.Auth.Dtos;
using Skimart.Application.Cases.Auth.Queries.CheckExistingEmail;
using Skimart.Application.Cases.Auth.Queries.GetUserAddress;
using Skimart.Application.Identity.Commands.Register;
using Skimart.Application.Identity.DTOs;
using Skimart.Application.Identity.Queries.GetCurrentLoggedUser;
using Skimart.Contracts.Identity.Requests;
using Skimart.Extensions.FluentResults;
using Skimart.Identity.Mappers;

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
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var command = loginRequest.ToCommand();
        var result = await _mediator.Send(command);
        
        return result.Match(
            user => Ok(user.ToResponse()),
            Problem);
    }
    
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> Register(RegisterCommand registerCommand)
    {
        var result = await _mediator.Send(registerCommand);
        
        return result.ToOkOrBadRequest(ErrorMessage);
    }
}