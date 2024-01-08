using Application.Abstractions.Persistence.Specifications;
using Domain.Entities;

namespace Application.Abstractions.Persistence.Repositories;

public interface IStoreRepository<T> where T : BaseEntity
{
    Task<IReadOnlyList<T>> GetEntitiesAsync();
    Task<IReadOnlyList<T>> GetEntitiesAsync(ISpecification<T> spec);
    Task<T> AddAsync(T entity);
    void UpdateAsync(T entity);
    void Delete(T entity);
    Task<T?> GetEntityByIdAsync(int id);
    Task<T?> GetEntityByIdAsync(ISpecification<T> spec);
    Task<int> CountAsync(ISpecification<T> spec);
}