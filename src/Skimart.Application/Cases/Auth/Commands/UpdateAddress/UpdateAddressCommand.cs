using System.Security.Claims;
using FluentResults;
using MediatR;
using Skimart.Application.Cases.Auth.Dtos;

namespace Skimart.Application.Cases.Auth.Commands.UpdateAddress;

public record UpdateAddressCommand(AddressDto AddressDto, ClaimsPrincipal Claims) : IRequest<Result<AddressDto>>;