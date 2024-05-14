using ErrorOr;
using MediatR;
using Skimart.Application.Identity.DTOs;

namespace Skimart.Application.Identity.Queries.GetCurrentLoggedUser;

public record GetCurrentLoggedUserQuery : IRequest<ErrorOr<CurrentUserDto>>;