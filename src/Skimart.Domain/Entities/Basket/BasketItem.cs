using Skimart.Domain.Entities.Products;

namespace Skimart.Domain.Entities.Basket;

public class BasketItem
{
    public int Id { get; }
    public string ProductName { get; }
    public decimal Price { get; private set; }
    public int Quantity { get; }
    public string PictureUrl { get; } 
    public string Brand { get; } 
    public string Type { get; }
    
    public BasketItem(int id, string productName, decimal price, int quantity, string pictureUrl, string brand, string type)
    {
        Id = id;
        ProductName = productName;
        Price = price;
        Quantity = quantity;
        PictureUrl = pictureUrl;
        Brand = brand;
        Type = type;
    }

    public void VerifyAgainstStoredPrice(Product product)
    {
        if (Price == product.Price) 
            return;

        Price = product.Price;
    }
}