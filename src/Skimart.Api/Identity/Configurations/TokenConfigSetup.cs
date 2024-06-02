using Microsoft.Extensions.Options;
using Skimart.Application.Identity.Configurations;

namespace Skimart.Identity.Configurations;

public class TokenConfigSetup : IConfigureOptions<TokenConfiguration>
{
    private const string SectionName = "Token";
    private readonly IConfiguration _configuration;

    public TokenConfigSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(TokenConfiguration options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}