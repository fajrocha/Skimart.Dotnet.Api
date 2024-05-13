using System.Security.Claims;
using FluentResults;
using MediatR;
using Skimart.Application.Cases.Auth.Dtos;

namespace Skimart.Application.Cases.Auth.Queries.GetUserAddress;

public record GetUserAddressQuery(ClaimsPrincipal Claims) : IRequest<Result<AddressDto>>;