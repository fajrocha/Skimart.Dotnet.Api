using FluentResults;
using MediatR;
using Microsoft.Extensions.Primitives;

namespace Skimart.Application.Cases.Payment.Commands.ConfirmPayment;

public record ConfirmPaymentCommand(string? BodyContent, StringValues PaymentEvent) : IRequest<Result>;