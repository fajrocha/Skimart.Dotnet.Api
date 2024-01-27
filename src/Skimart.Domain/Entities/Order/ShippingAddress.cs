namespace Skimart.Domain.Entities.Order;

public class ShippingAddress
{
    public ShippingAddress()
    {
    }

    public ShippingAddress(string firstName, string lastName, string street, string city, string province, string zipCode)
    {
        FirstName = firstName;
        LastName = lastName;
        Street = street;
        City = city;
        Province = province;
        ZipCode = zipCode;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string Province { get; set; }
    public string ZipCode { get; set; }
}