using MediatR;

namespace Skimart.Application.Cases.Auth.Queries.CheckExistingEmail;

public record CheckExistingEmailQuery(string Email) : IRequest<bool>;
