using ErrorOr;
using MediatR;
using Skimart.Domain.Entities.Auth;

namespace Skimart.Application.Identity.Queries.GetUserAddress;

public record GetUserAddressQuery : IRequest<ErrorOr<Address>>;