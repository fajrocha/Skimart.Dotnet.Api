using Skimart.Domain.Entities;

namespace Skimart.Application.Shared.Gateways;

public interface IUnitOfWork : IDisposable
{
    TRepos Repository<TRepos, TEntity>() 
        where TRepos : IBaseRepository<TEntity>
        where TEntity : BaseEntity;

    Task<int> CompleteAsync();
}