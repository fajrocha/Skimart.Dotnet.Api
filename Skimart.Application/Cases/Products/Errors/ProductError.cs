using FluentResults;

namespace Skimart.Application.Cases.Products.Errors;

public class ProductError : Error
{
    private ProductError(string message) : base(message)
    { 
    }

    public static ProductError NotFound => new ProductError("The product requested was not found.");
}