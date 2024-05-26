using System.Linq.Expressions;
using Gym.Domain.Commons;

namespace Gym.Repository.Repositories;

public interface IGRepository<T> where T : Auditable
{
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(Expression<Func<T, bool>> expression);
    Task<T> SelectAsync(Expression<Func<T, bool>> expression, string[] includes = null);
    IQueryable<T> SelectAll(Expression<Func<T, bool>> expression = null, string[] includes = null, bool isTracking = true);
    Task SaveAsync();
}