using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skimart.Application.Identity.Queries.CheckExistingEmail;
using Skimart.Application.Identity.Queries.GetCurrentLoggedUser;
using Skimart.Application.Identity.Queries.GetUserAddress;
using Skimart.Contracts.Identity.Requests;
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
    public async Task<IActionResult> GetCurrentUser()
    {
        var query = new GetCurrentLoggedUserQuery();
        var result = await _mediator.Send(query);

        return result.Match(
            currentUser => Ok(currentUser.ToResponse()),
            Problem);
    }
    
    [HttpGet("email")]
    [AllowAnonymous]
    public async Task<IActionResult> CheckEmailExists([FromQuery] string email)
    {
        var query = new CheckExistingEmailQuery(email);
        
        return Ok(await _mediator.Send(query));
    }
    
    [HttpGet("address")]
    public async Task<IActionResult> GetAddress()
    {
        var query = new GetUserAddressQuery();
        var result = await _mediator.Send(query);

        return result.Match(
            address => Ok(address.ToResponse()),
            Problem);
    }
    
    [HttpPut("address")]
    public async Task<IActionResult> UpdateAddress(AddressUpdateRequest addressUpdateRequest)
    {
        var command = addressUpdateRequest.ToCommand();
        var result = await _mediator.Send(command);

        return result.Match(
            address => Ok(address.ToResponse()),
            Problem);
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
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        var command = registerRequest.ToCommand();
        var result = await _mediator.Send(command);
        
        return result.Match(
            user => Ok(user.ToResponse()),
            Problem);
    }
}