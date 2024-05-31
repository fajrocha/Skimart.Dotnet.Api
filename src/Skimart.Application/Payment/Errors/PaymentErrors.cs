namespace Skimart.Application.Payment.Errors;

public static class PaymentErrors
{
    public static string InvalidPaymentWebhookBody => "Body of payment webhook request is null or empty.";
    public static string FailedToUpdateOrder => "Failed to update the order with intent.";
}