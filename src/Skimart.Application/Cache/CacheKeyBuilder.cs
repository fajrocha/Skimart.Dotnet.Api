using System.Text;

namespace Skimart.Application.Cache;

public class CacheKeyBuilder
{
    private string _prefix = string.Empty;
    private string _name = string.Empty;

    public CacheKeyBuilder WithName(string name)
    {
        _name = name;

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
        var separator = !string.IsNullOrEmpty(_prefix) ? ":" : string.Empty;

        return keyBuilder.Append(_prefix)
            .Append(separator)
            .Append(_name)
            .ToString();
    }

    public static string KeyGetById<T>()
    {
        return $"GetEntityById{typeof(T).Name}";
    }
    
    public static string KeyGetById(int id)
    {
        return $"Id:{id}";
    }
    
    public static string KeyGetAll<T>()
    {
        return $"GetEntities{typeof(T).Name}";
    }
}