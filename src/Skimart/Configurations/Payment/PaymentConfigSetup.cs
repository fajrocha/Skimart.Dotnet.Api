using Microsoft.Extensions.Options;
using Skimart.Application.Configurations.Payment;

namespace Skimart.Configurations.Payment;

public class PaymentConfigSetup : IConfigureOptions<PaymentConfiguration>
{
    private const string SectionName = "PaymentService";
    private readonly IConfiguration _configuration;

    public PaymentConfigSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(PaymentConfiguration options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}