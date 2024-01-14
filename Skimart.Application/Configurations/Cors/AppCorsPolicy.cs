namespace Skimart.Application.Configurations.Cors;

public class AppCorsPolicy
{
    public string PolicyName { get; set; } = string.Empty;
    public string[] Origins { get; set; } = Array.Empty<string>();
}