using ErrorOr;
using MediatR;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.Payment.Commands.CreateOrUpdatePaymentIntent;

public record CreateOrUpdatePaymentIntentCommand(string BasketId) : IRequest<ErrorOr<CustomerBasket>>;