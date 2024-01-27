using System.Collections;
using Skimart.Application.Abstractions.Persistence.Repositories;
using Skimart.Domain.Entities;
using Skimart.Infrastructure.Persistence.DbContexts;

namespace Skimart.Infrastructure.Persistence.Repositories;

public class StoreUnitOfWork : IUnitOfWork
{
    private bool _disposing;
    private readonly StoreContext _context;
    private readonly IServiceProvider _serviceProvider;
    private readonly Hashtable _repositories;

    public StoreUnitOfWork(StoreContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
        _repositories = new Hashtable();
    }
    
    public TRepos Repository<TRepos, TEntity>() 
        where TRepos : IBaseRepository<TEntity> 
        where TEntity : BaseEntity
    {
        var type = typeof(TEntity).Name;

        if (_repositories.ContainsKey(type))
            return (TRepos?)_repositories[type] 
                   ?? throw new ArgumentException($"Entity {type} is not a valid entity.");
        
        var service = _serviceProvider.GetService(typeof(TRepos));
        _repositories.Add(type, service);

        return (TRepos?)service ?? throw new ArgumentException($"Service for entity {type} is null.");
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }
    
    protected virtual void Dispose(bool disposing)
    {
        _disposing = disposing;

        if (disposing)
        {
            _context.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}