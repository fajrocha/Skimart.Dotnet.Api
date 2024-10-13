using ErrorOr;
using MediatR;
using Skimart.Application.Identity.DTOs;

namespace Skimart.Application.Identity.Commands.Register;

public record RegisterCommand(string DisplayName, string Email, string Password) : IRequest<ErrorOr<UserDto>>;