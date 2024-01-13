namespace Skimart.Application.Configurations.Memory;

public class CacheConfig
{
    public int ProductsTimeToLive { get; init; } = 60;
    public int BrandsTimeToLive { get; init; } = 600;
    public int TypesTimeToLive { get; init; } = 600;
}