using Microsoft.Extensions.Options;
using Skimart.Application.Configurations.Memory;

namespace Skimart.Configurations.Memory;

public class CacheConfigSetup : IConfigureOptions<CacheConfiguration> 
{
    private const string SectionName = "CacheConfiguration";
    private readonly IConfiguration _configuration;

    public CacheConfigSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(CacheConfiguration options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}