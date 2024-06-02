namespace Skimart.Shared.Extensions;

public static class ApiUrl
{
    public static string CombineWithApiUrl(this string content)
    {
        var apiUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Split(';').FirstOrDefault()
                     ?? string.Empty;
        
        return $"{apiUrl}/{content}";
    }
}