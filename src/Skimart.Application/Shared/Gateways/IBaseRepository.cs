using Skimart.Domain.Entities;

namespace Skimart.Application.Shared.Gateways;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<List<T>> GetEntitiesAsync();
    Task<T> AddAsync(T entity);
    void UpdateAsync(T entity);
    void Delete(T entity);
    Task<T?> GetEntityByIdAsync(int id);
    Task<int> SaveChangesAsync();
}