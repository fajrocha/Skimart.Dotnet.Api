namespace Skimart.Application.Cases.Payment.Commands.ConfirmPayment;

public record PaymentResult(bool IsSuccess, string PaymentIntent)
{
    public static PaymentResult SuccessPayment(string paymentIntent) => new(true, paymentIntent);
    public static PaymentResult FailedPayment(string paymentIntent) => new(false, paymentIntent);
}