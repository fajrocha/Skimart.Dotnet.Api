using MediatR;

namespace Skimart.Application.Identity.Queries.CheckExistingEmail;

public record CheckExistingEmailQuery(string Email) : IRequest<bool>;
