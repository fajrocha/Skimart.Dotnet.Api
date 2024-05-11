using Skimart.Domain.Entities;

namespace Skimart.Application.Gateways.Persistence.Repositories;

public interface IUnitOfWork : IDisposable
{
    TRepos Repository<TRepos, TEntity>() 
        where TRepos : IBaseRepository<TEntity>
        where TEntity : BaseEntity;

    Task<int> CompleteAsync();
}