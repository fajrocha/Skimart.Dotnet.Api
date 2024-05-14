using Microsoft.EntityFrameworkCore;
using Skimart.Application.Cache.Gateways;
using Skimart.Application.Gateways.Persistence.Repositories;
using Skimart.Application.Gateways.Persistence.Specifications;
using Skimart.Domain.Entities;
using Skimart.Infrastructure.Persistence.DbContexts;
using Skimart.Infrastructure.Persistence.Specifications;

namespace Skimart.Infrastructure.Persistence.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    private readonly StoreContext _context;

    protected BaseRepository(StoreContext context)
    {
        _context = context;
    }
    
    public async Task<List<T>> GetEntitiesAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }
    
    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);

        return entity;
    }

    public void UpdateAsync(T entity)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
    
    public async Task<T?> GetEntityByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
    
    protected async Task<List<T>> GetEntitiesAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    protected async Task<T?> GetEntityByIdAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    protected async Task<int> CountAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).CountAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
    }
}

public class BaseCacheRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    private readonly IBaseRepository<T> _decorated;
    private readonly ICacheService _cacheService;

    protected BaseCacheRepository(
        IBaseRepository<T> decorated,
        ICacheService cacheService)
    {
        _decorated = decorated;
        _cacheService = cacheService;
    }
    
    public async Task<List<T>> GetEntitiesAsync()
    {
        return await _cacheService.GetOrCacheValueAsync(
            $"GetEntities{typeof(T)}-",
            async () => await _decorated.GetEntitiesAsync(),
            TimeSpan.FromSeconds(60)) ?? new List<T>();
    }

    public Task<T> AddAsync(T entity)
    {
        return _decorated.AddAsync(entity);
    }

    public void UpdateAsync(T entity)
    {
        _decorated.UpdateAsync(entity);
    }

    public void Delete(T entity)
    {
        _decorated.Delete(entity);
    }

    public async Task<T?> GetEntityByIdAsync(int id)
    {
        return await _cacheService.GetOrCacheValueAsync(
            $"GetEntityById{typeof(T)}-",
            async () => await _decorated.GetEntityByIdAsync(id),
            TimeSpan.FromSeconds(60));
    }

    public Task<int> SaveChangesAsync()
    {
        return _decorated.SaveChangesAsync();
    }
}