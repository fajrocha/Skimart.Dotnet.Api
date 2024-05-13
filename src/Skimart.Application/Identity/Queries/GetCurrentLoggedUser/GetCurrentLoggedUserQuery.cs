using System.Security.Claims;
using FluentResults;
using MediatR;
using Skimart.Application.Identity.DTOs;

namespace Skimart.Application.Identity.Queries.GetCurrentLoggedUser;

public record GetCurrentLoggedUserQuery(ClaimsPrincipal Claims) : IRequest<Result<UserDto>>;