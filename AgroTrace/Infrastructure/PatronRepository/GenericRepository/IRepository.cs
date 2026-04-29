using System.Linq.Expressions;

namespace AgroTrace.Infrastructure.PatronRepository.GenericRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> Exists(Expression<Func<T, bool>> predicate);


    }
}

