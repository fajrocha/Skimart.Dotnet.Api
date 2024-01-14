using FluentResults;

namespace Skimart.Application.Cases.Basket.Errors;

public class CustomerBasketError : Error
{
    private CustomerBasketError(string message) : base(message)
    { 
    }

    public static CustomerBasketError NotFound => new CustomerBasketError("The basket requested was not found.");
    
    public static CustomerBasketError UpdateOrCreateFailed => 
        new CustomerBasketError("The operation to create or update the basket failed.");
}