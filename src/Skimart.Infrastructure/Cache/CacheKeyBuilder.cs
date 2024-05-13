using System.Text;

namespace Skimart.Infrastructure.Cache;

public class CacheKeyBuilder
{
    private string _prefix = string.Empty;
    private string _key = string.Empty;

    public CacheKeyBuilder WithKey(string key)
    {
        _key = key;

        return this;
    }
    
    public CacheKeyBuilder WithPrefix(string prefix)
    {
        _prefix = prefix;

        return this;
    }

    public string Build()
    {
        var keyBuilder = new StringBuilder();

        return keyBuilder.Append(_prefix)
            .Append(_key)
            .ToString();
    }

    public static string GetById<T>()
    {
        return $"GetEntityById{typeof(T).Name}-";
    }
    
    public static string GetAll<T>()
    {
        return $"GetEntities{typeof(T).Name}-";
    }
}