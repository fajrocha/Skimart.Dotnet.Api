using ErrorOr;
using MediatR;
using Skimart.Domain.Entities.Auth;

namespace Skimart.Application.Identity.Commands.UpdateAddress;

public record UpdateAddressCommand(
    string FirstName,
    string LastName, 
    string Street, 
    string City, 
    string Province, 
    string ZipCode): IRequest<ErrorOr<Address>>;