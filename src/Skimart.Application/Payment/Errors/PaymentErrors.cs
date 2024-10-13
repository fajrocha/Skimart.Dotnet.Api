namespace Skimart.Application.Payment.Errors;

public static class PaymentErrors
{
    public static string BasketNotFound => "Customer basket was not found.";
    public static string ProductNotFound(int itemId) => $"Product with id {itemId} was not found.";
    public static string NoDeliveryMethodOnBasket => "No delivery method on basket.";
    public static string DeliveryMethodNotFound => "Delivery method was not found on storage.";
    public static string InvalidPaymentWebhookBody => "Body of payment webhook request is null or empty.";
    public static string FailedToUpdateOrder => "Failed to update the order with intent.";
}