using AgroTrace.Aplication.DTO;
using AgroTrace.Domain.Entities;
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

        // Obtener 
        public async Task<(List<T>, int)> Obtener(
          Filtro filtro,
          Expression<Func<T, bool>>? predicate = null,
          Expression<Func<T, object>>? orderBy = null
 )
        {
            var query = _db.AsNoTracking().AsQueryable();

            // Filtro dinámico
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            //Ordenamiento
            if (orderBy != null)
            {
                query = filtro.Descendente
                    ? query.OrderByDescending(orderBy)
                    : query.OrderBy(orderBy);
            }

            //Total antes de paginar
            var total = await query.CountAsync();

            //Paginación segura
            var pageNumber = filtro.PageNumber <= 0 ? 1 : filtro.PageNumber;
            var pageSize = filtro.PageSize <= 0 ? 10 : filtro.PageSize;

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, total);
        }

    }
}
