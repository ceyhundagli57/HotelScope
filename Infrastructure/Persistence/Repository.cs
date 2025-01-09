using System.Linq.Expressions;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class Repository <T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> dbSet;

    public Repository(ApplicationDbContext db)
    {
        _db = db;
        dbSet = _db.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null,
        bool tracked = false)
    {
        IQueryable<T> query = tracked ? dbSet : dbSet.AsNoTracking();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' },
                         StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }

        return await query.ToListAsync();
    }

    public async Task<T?> GetAsync(Guid id, string? includeProperties = null, bool tracked = false)
    {
        IQueryable<T> query = tracked ? dbSet : dbSet.AsNoTracking();

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' },
                         StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }

        return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
    }

    public async Task<T> AddAsync(T entity)
    {
        await dbSet.AddAsync(entity);
        return entity;
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }

    public void Update(T entity)
    {
        dbSet.Update(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        dbSet.RemoveRange(entities);
    }

    
}