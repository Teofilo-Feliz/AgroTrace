using AgroTrace.Aplication.DTO;
using AgroTrace.Domain.Entities;
using AgroTrace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgroTrace.Infrastructure.PatronRepository.AnimalRepository
{
    public class AnimalRepository: IAnimalRepository
    {
        private readonly AppDbContext _context;

        public AnimalRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<(List<Animal>, int)> ObtenerAnimales(FiltroAnimal filtro)
        {
            var query = _context.Animales
       .Include(a => a.Finca)
       .Include(a => a.TipoAnimal)
       .Include(a => a.Raza)
       .Include(a => a.EstadoAnimal)
       .AsNoTracking()
       .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro.Codigo))
            {
                var codigo = filtro.Codigo.Trim();
                query = query.Where(a => a.Codigo.Contains(codigo));
            }

            if (filtro.FincaId.HasValue)
            {
                query = query.Where(a => a.FincaId == filtro.FincaId.Value);
            }

            if (filtro.Activo.HasValue)
            {
                query = query.Where(a => a.Activo == filtro.Activo.Value);
            }

            var total = await query.CountAsync();

            var data = await query
                .OrderBy(a => a.Id)
                .Skip((filtro.PageNumber - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .ToListAsync();

            return (data, total);


        }

        public async Task<bool> ExisteCodigo(string codigo, int fincaId)
        {
            return await _context.Animales
             .AnyAsync(a => a.Codigo == codigo && a.FincaId == fincaId);
        }


        public async Task AgregarAnimal(Animal animal)
        {
            await _context.Animales.AddAsync(animal);

        }

    }
}

  