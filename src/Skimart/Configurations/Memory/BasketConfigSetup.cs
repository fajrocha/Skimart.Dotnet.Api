using Microsoft.Extensions.Options;
using Skimart.Application.Configurations.Memory;

namespace Skimart.Configurations.Memory;

public class BasketConfigSetup : IConfigureOptions<CacheConfiguration>
{
    private const string SectionName = "BasketRepository";
    private readonly IConfiguration _configuration;

    public BasketConfigSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void Configure(CacheConfiguration options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}