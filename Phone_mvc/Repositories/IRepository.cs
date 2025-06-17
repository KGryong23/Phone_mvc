using Phone_mvc.Common;
using System.Linq.Expressions;

namespace Phone_mvc.Repositories
{
    public interface IRepository<T> where T : class
    {
        T GetById(Guid id);
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        T Update(T entity);
        void Delete(T entity);
        Task<bool> SaveChangesAsync();
        Task<PagedResult<T>> GetPagedAsync(BaseQuery query, string searchField);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<T?> FindFirstAsync(Expression<Func<T, bool>> exp);
    }
}
