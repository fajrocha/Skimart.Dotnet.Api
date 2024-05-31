namespace Skimart.Application.Basket.Errors;

public static class BasketErrors
{
    public static string BasketNotFound => "Basket requested was not found.";
    public static string FailedToUpdateOrCreateBasket => "Failed to create or update the basket.";
    public static string FailedToDeleteBasket => "Failed to delete the basket.";
}