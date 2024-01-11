using System.Text.Json;

namespace Skimart.Application.Helpers;

public static class SystemJsonSerializer
{
    public static string? SerializeCamelCase<T>(T objectToSerialize)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Serialize(objectToSerialize, options);
    }

    public static T DeserializeCamelCase<T>(string objectToDeserialize)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Deserialize<T>(objectToDeserialize, options) 
               ?? throw new InvalidOperationException($"Could not deserialize object of type {typeof(T)}");
    }
}