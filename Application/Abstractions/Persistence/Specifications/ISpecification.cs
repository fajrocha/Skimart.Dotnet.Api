using System.Linq.Expressions;

namespace Application.Abstractions.Persistence.Specifications;

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