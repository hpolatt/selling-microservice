using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.SeedWork;
using OrderService.Infrastructure.Context;

namespace OrderService.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    public IUnitOfWork UnitOfWork { get; }

    private readonly OrderDbContext dbContext;

    public GenericRepository(OrderDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await dbContext.Set<T>().AddAsync(entity);
        return entity;
    }
    public virtual Task<List<T>> Get(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
    {
        return Get(filter, null, includes);
    }

    public virtual async Task<List<T>> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = dbContext.Set<T>();

        foreach (Expression<Func<T, object>> include in includes)
            query = query.Include(include);

        if (filter != null)
            query = query.Where(filter);
        if (orderBy != null)
            query = orderBy(query);

        return await query.ToListAsync();
    }



    public virtual async Task<List<T>> GetAll()
    {
        return await dbContext.Set<T>().ToListAsync();
    }

    public virtual async Task<T> GetById(Guid id)
    {
        return await dbContext.Set<T>().FindAsync(id);
    }

    public virtual Task<T> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = dbContext.Set<T>();
        foreach (Expression<Func<T, object>> include in includes)
            query = query.Include(include);

        return query.FirstOrDefaultAsync(x => x.Id == id);
    }

    public virtual Task<T> GetSingleAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = dbContext.Set<T>();
        foreach (Expression<Func<T, object>> include in includes)
            query = query.Include(include);

        return query.FirstOrDefaultAsync(expression);
    }

    public virtual T Update(T entity)
    {
        dbContext.Set<T>().Update(entity);
        return entity;
    }
}