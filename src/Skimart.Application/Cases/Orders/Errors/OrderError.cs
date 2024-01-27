using FluentResults;

namespace Skimart.Application.Cases.Orders.Errors;

public class OrderError : Error
{
    private OrderError(string message) : base(message)
    { 
    }

    public static OrderError BasketNotFound => new("Could not find the basket.");
    public static OrderError ProductIdNotFound(int id) => new($"Could not find the product with id {id}.");
    public static OrderError DeliveryMethodNotFound(int id) => new($"Could not find the delivery method with id {id}.");
    public static OrderError OrderNotFound(int id) => new($"Could not find the order with id {id}.");
    public static OrderError OrderCreateTransactionFailed => new($"Transaction to create the order failed.");
    public static OrderError OrderUpdateTransactionFailed => new($"Transaction to update the order failed.");
}
