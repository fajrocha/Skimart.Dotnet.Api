using Microsoft.Extensions.Options;
using Skimart.Application.Configurations.Memory;

namespace Skimart.Configurations.Memory;

public class CacheConfigSetup : IConfigureOptions<CacheConfig> 
{
    private const string SectionName = "CacheService";
    private readonly IConfiguration _configuration;

    public CacheConfigSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(CacheConfig options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}