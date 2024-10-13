using System.Linq.Expressions;

namespace Skimart.Application.Shared.Gateways;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Filters { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDescending { get; }
    int Take { get; }
    int Skip { get; }
    bool IsPaginated { get; }
}