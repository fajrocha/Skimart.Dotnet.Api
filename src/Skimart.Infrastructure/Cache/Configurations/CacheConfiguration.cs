namespace Skimart.Infrastructure.Cache.Configurations;

public class CacheConfiguration
{
    public int ProductsTimeToLiveSecs { get; init; } = 60;
    public int BrandsTimeToLiveSecs { get; init; } = 180;
    public int TypesTimeToLiveSecs { get; init; } = 180;
}