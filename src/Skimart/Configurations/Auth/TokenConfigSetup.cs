using Microsoft.Extensions.Options;
using Skimart.Application.Configurations.Auth;

namespace Skimart.Configurations.Auth;

public class TokenConfigSetup : IConfigureOptions<TokenConfig>
{
    private const string SectionName = "Token";
    private readonly IConfiguration _configuration;

    public TokenConfigSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(TokenConfig options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}