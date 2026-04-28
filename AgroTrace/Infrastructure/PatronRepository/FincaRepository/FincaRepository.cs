using AgroTrace.Aplication.DTO;
using AgroTrace.Aplication.Interfaces;
using AgroTrace.Domain.Entities;
using AgroTrace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgroTrace.Infrastructure.PatronRepository.FincaRepository
{
    public class FincaRepository: IFincaRepository
    {
        private readonly AppDbContext _context;

        public FincaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Finca>, int )> ObtenerFincas(Filtro filtro)
        {
            var query = _context.Fincas
               .Include(f => f.UsuarioPropietario) 
               .AsNoTracking()
               .AsQueryable();

            if (!string.IsNullOrEmpty(filtro.Buscar))
            {
                var buscar = filtro.Buscar.Trim();
                query = query.Where(f => f.Nombre.Contains(buscar));
            }

            query = filtro.Descendente
                ? query.OrderByDescending(r => r.Id)
                : query.OrderBy(r => r.Id);
            var total = await query.CountAsync();


            var pageNumber = filtro.PageNumber <= 0 ? 1 : filtro.PageNumber;
            var pageSize = filtro.PageSize <= 0 ? 10 : filtro.PageSize;

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, total);

        }




        public async Task AgregarFinca(Finca finca)
        {
            await _context.AddAsync(finca);
        }

        public async Task<bool> ExisteUsuario(int usuarioPropiedadId)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Id == usuarioPropiedadId);
        }

        public async Task<string?> ObtenerNombreUsuarioPropiedad(int usuarioId)
        {
            return await _context.Usuarios
                .Where(u => u.Id == usuarioId)
                .Select(u => u.Nombre)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> FincaExiste(string nombre, int usuarioId)
        {
            nombre = nombre.Trim().ToLower();

            return await _context.Fincas
                .AnyAsync(f =>
                    f.UsuarioPropietarioId == usuarioId &&
                    f.Nombre.ToLower() == nombre
                );
        }
    }
}
