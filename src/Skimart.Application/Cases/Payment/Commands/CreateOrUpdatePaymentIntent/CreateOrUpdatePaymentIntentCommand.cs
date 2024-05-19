using ErrorOr;
using FluentResults;
using MediatR;
using Skimart.Domain.Entities.Basket;

namespace Skimart.Application.Cases.Payment.Commands.CreateOrUpdatePaymentIntent;

public record CreateOrUpdatePaymentIntentCommand(string BasketId) : IRequest<ErrorOr<CustomerBasket>>;