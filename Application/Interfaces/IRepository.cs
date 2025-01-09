using System.Linq.Expressions;

namespace Application.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null,
        bool tracked = false);    
    Task<T?> GetAsync(Guid id, string? includeProperties = null, bool tracked = false);
    Task<T> AddAsync(T entity);
    void Remove(T entity);
    void Update(T entity);
    void RemoveRange(IEnumerable<T> entities);

    
}