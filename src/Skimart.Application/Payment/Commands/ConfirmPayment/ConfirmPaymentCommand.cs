using ErrorOr;
using MediatR;
using Microsoft.Extensions.Primitives;
using Success = ErrorOr.Success;

namespace Skimart.Application.Payment.Commands.ConfirmPayment;

public record ConfirmPaymentCommand(string? BodyContent, StringValues PaymentEvent) : IRequest<ErrorOr<Success>>;