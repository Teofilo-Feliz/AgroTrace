using AgroTrace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AgroTrace.Infrastructure.PatronRepository.GenericRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _db;

        public Repository(AppDbContext context)
        {
            _context = context;
            _db = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
            => await _db.FindAsync(id);

        public async Task AddAsync(T entity)
            => await _db.AddAsync(entity);

        public void Update(T entity)
            => _db.Update(entity);

        public void Delete(T entity)
            => _db.Remove(entity);

        // Saber si el producto que se ingresa existe.
        public async Task<bool> Exists(Expression<Func<T, bool>> predicate)
        {
            return await _db.AnyAsync(predicate);
        }
    }
}
