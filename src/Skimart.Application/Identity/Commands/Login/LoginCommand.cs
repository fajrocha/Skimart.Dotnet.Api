using ErrorOr;
using MediatR;
using Skimart.Application.Identity.DTOs;

namespace Skimart.Application.Identity.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<ErrorOr<UserDto>>;