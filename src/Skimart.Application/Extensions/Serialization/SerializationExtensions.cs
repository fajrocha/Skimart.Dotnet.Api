using System.Text.Json;

namespace Skimart.Application.Extensions.Serialization;

public static class SerializationExtensions
{
    public static T DeserializeCamelCase<T>(this string objectToDeserialize)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Deserialize<T>(objectToDeserialize, options) 
               ?? throw new InvalidOperationException($"Could not deserialize object of type {typeof(T)}");
    }
}