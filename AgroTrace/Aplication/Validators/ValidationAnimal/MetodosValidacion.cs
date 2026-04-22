using AgroTrace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AgroTrace.Aplication.Validators.ValidationAnimal
{
    public class MetodosValidacion
    {
        private readonly AppDbContext _context;

        public MetodosValidacion(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExisteAsync<T>(Expression<Func<T, bool>> filtro) where T : class
        {
            return await _context.Set<T>().AnyAsync(filtro);
        }







    }
}
