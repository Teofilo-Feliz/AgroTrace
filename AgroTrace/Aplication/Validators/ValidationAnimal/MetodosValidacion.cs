using AgroTrace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> FincaExiste(int id)
        {
            return await _context.Fincas.AnyAsync(f => f.Id == id);
        }

        public async Task<bool> TipoAnimalExiste(int id)
        {
            return await _context.TipoAnimales.AnyAsync(anim => anim.Id == id);
        }

        public async Task<bool> RazaIdExiste(int id)
        {
            return await _context.Razas.AnyAsync(R => R.Id == id);
        }

        public async Task<bool> EstadoAnimalId(int id)
        {
            return await _context.EstadoAnimales.AnyAsync(E => E.Id == id);
        }










    }
}
