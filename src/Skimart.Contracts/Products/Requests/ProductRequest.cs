namespace Skimart.Contracts.Products.Requests;

public class ProductRequest
{
    private const int MaxPageSize = 50;
    private int _pageSize = 6;
    public int PageIndex { get; init; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public int? BrandId { get; init; }
    public int? TypeId { get; init; }
    public string? Sort { get; init; }

    private string _search = string.Empty;

    public string Search
    {
        get => _search;
        set => _search = value.ToLower();
    }
}