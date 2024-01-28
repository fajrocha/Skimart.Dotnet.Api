using FluentResults;

namespace Skimart.Application.Cases.Payment.Errors;

public class PaymentError : Error
{
    private PaymentError(string message) : base(message)
    {
    }      
    
    public static PaymentError DeliveryMethodNotFound => new("Could not find the delivery method.");
    public static PaymentError ProductIdNotFound(int id) => new($"Could not find the product with id {id}.");
    public static PaymentError InvalidPaymentWebhookBody => new("Body of payment webhook request is null or empty");
}