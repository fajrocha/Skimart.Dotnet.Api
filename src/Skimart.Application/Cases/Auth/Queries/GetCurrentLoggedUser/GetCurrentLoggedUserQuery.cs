using System.Security.Claims;
using FluentResults;
using MediatR;
using Skimart.Application.Cases.Auth.Dtos;

namespace Skimart.Application.Cases.Auth.Queries.GetCurrentLoggedUser;

public record GetCurrentLoggedUserQuery(ClaimsPrincipal Claims) : IRequest<Result<UserDto>>;