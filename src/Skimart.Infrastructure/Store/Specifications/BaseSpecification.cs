using System.Linq.Expressions;
using Skimart.Application.Shared.Gateways;

namespace Skimart.Infrastructure.Store.Specifications;

public class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>>? Filters { get; }
    public List<Expression<Func<T, object>>> Includes { get; } = new();
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set;}
    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPaginated { get; private set; }
    
    public BaseSpecification()
    {
    }

    public BaseSpecification(Expression<Func<T, bool>> filters)
    {
        Filters = filters;
    }
    
    protected void AddInclude(Expression<Func<T, object>> expression)
    {
        Includes.Add(expression);
    }

    protected void AddOrderBy(Expression<Func<T, object>> orderBy)
    {
        OrderBy = orderBy;
    }

    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescending)
    {
        OrderByDescending = orderByDescending;
    }

    protected void ApplyPagination(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPaginated = true;
    }
}