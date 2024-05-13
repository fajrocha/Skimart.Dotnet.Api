﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Skimart.Application.Extensions.Serialization;

public static class SerializationExtensions
{
    public static T DeserializeCamelCase<T>(this string objectToDeserialize)
    {
        return JsonSerializer.Deserialize<T>(objectToDeserialize, DefaultOptions) 
               ?? throw new InvalidOperationException($"Could not deserialize object of type {typeof(T)}");
    }
    
    public static string SerializeCamelCase<T>(this T objectToSerialize)
    {
        return JsonSerializer.Serialize(objectToSerialize, DefaultOptions);
    }
    
    private static JsonSerializerOptions DefaultOptions  => new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}