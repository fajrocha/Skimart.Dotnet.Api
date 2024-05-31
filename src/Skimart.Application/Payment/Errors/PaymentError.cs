using FluentResults;

namespace Skimart.Application.Payment.Errors;

public static class PaymentError
{
    public static string DeliveryMethodNotFound => "Could not find the delivery method.";
    public static string ProductIdNotFound(int id) => $"Could not find the product with id {id}.";
    public static string InvalidPaymentWebhookBody => "Body of payment webhook request is null or empty.";
    public static string FailedToUpdateOrder => "Failed to update the order with intent.";
}