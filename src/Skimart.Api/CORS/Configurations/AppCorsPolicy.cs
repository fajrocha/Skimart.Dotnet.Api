namespace Skimart.Cors.Configurations;

public class AppCorsPolicy
{
    public string PolicyName { get; init; } = string.Empty;
    public string[] Origins { get; init; } = Array.Empty<string>();
}