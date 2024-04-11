using ms.MainApi.Core.Interfaces;
using System.Linq.Expressions;

namespace ms.MainApi.Core.DataAccess;

public interface IEntityRepository<T> where T : class, IEntity, new()
{
    Task<List<T>> GetAllQueryAsync(string? Query = null);
    List<T> GetAllQuery(ref int totalItems, int page, int pageSize, string? Query = null);
    List<T> GetAll(Expression<Func<T, bool>> filter = null);
    T? Get(Expression<Func<T, bool>> filter);
    bool Any(Expression<Func<T, bool>> filter);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    int Count(Expression<Func<T, bool>> filter = null);

    // async
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
    Task<List<T>> GetAllPaginationAsync(int page, int pageSize, Expression<Func<T, bool>> filter = null);
    Task<T?> GetAsync(Expression<Func<T, bool>> filter);
    Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(List<T> entities);
    Task DeleteAsync(Expression<Func<T, bool>> filter = null);
    Task<int> CountAsync(Expression<Func<T, bool>> filter = null);
}
