using AgroTrace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
    }
}
