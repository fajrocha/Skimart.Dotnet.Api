using Application.Abstractions.Persistence.Repositories;
using Application.Abstractions.Persistence.Specifications;
using Domain.Entities;
using Infrastructure.Persistence.DbContexts;
using Infrastructure.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class StoreRepository<T> : IStoreRepository<T> where T : BaseEntity
{
    private readonly StoreContext _context;

    protected StoreRepository(StoreContext context)
    {
        _context = context;
    }
    
    public async Task<IReadOnlyList<T>> GetEntitiesAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }
    
    public async Task<IReadOnlyList<T>> GetEntitiesAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
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
    
    public async Task<T?> GetEntityByIdAsync(ISpecification<T> spec)
    {
        return (await ApplySpecification(spec).FirstOrDefaultAsync())!;
    }

    public async Task<int> CountAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).CountAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
    }
}