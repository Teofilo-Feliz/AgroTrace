using AgroTrace.Aplication.DTO;
using AgroTrace.Domain.Entities;
using AgroTrace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AgroTrace.Infrastructure.PatronRepository.RolesRepository
{
    public class RolesRepository: IRolesRepository
    {
        private readonly AppDbContext _context;

        public RolesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Rol>, int)> ObtenerRoles(Filtro filtro)
        {
            var query = _context.Roles
       .AsNoTracking()
       .AsQueryable();

         
            if (!string.IsNullOrWhiteSpace(filtro.Buscar))
            {
                var buscar = filtro.Buscar.Trim();
                query = query.Where(r => r.Nombre.Contains(buscar));
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

        public async Task<bool> ExisteRol(string nombre)
        {
            return await _context.Roles
          .AnyAsync(r => r.Nombre == nombre);
        }

        public async Task<bool> ExisteRolId(string nombre, int id)
        {
            return await _context.Roles
                .AnyAsync(r => r.Nombre == nombre && r.Id != id);
        }

        public async Task<Rol?> ValidarPorId(int id)
        {
            return await _context.Roles.FindAsync(id);
        }




        public async Task AgregarRol(Rol roles)
        {
        

            var agregarRol = _context.AddAsync(roles);
            
        }

        public async Task<Rol?> ObtenerRolId(int id)
        {
           return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }


    }
}
