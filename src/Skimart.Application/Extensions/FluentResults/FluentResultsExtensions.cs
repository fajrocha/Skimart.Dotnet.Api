using FluentResults;

namespace Skimart.Application.Extensions.FluentResults;

public static class FluentResultsExtensions
{
    public static IEnumerable<string> GetReasonsAsCollection(this Result result)
    {
        return result.Reasons.Select(r => r.Message);
    }

    public static IEnumerable<string> GetReasonsAsCollection<T>(this Result<T> result)
    {
        return result.Reasons.Select(r => r.Message);
    }
}