using Microsoft.EntityFrameworkCore;
using Skimart.Application.Gateways.Persistence.Specifications;
using Skimart.Domain.Entities;

namespace Skimart.Infrastructure.Persistence.Specifications;

public static class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> entityQueryable, ISpecification<TEntity> spec)
    {
        if (spec.Filters is not null)
            entityQueryable = entityQueryable.Where(spec.Filters);

        if (spec.OrderBy is not null)
            entityQueryable = entityQueryable.OrderBy(spec.OrderBy);

        if (spec.OrderByDescending is not null)
            entityQueryable = entityQueryable.OrderByDescending(spec.OrderByDescending);

        if (spec.IsPaginated)
            entityQueryable = entityQueryable.Skip(spec.Skip).Take(spec.Take);

        return spec.Includes.Aggregate(entityQueryable, (current, include) => current.Include(include));
    }
}