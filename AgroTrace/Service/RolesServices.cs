


using AgroTrace.DTO;
using AgroTrace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgroTrace.Service
{
    public class RolesServices: IRoles     
    {
        private readonly AppDbContext _context;

        public RolesServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<RolesResponse>>> ObtenerRoles()
        {
            var response = new Response<List<RolesResponse>>
            {
                Errors = new List<string>()
            };

            try
            {
                var roles = await _context.Roles
                    .Select(r => new RolesResponse
                    {
                        Id = r.Id,
                        Nombre = r.Nombre,
                        Descripcion = r.Descripcion,      
                        Activo = r.Activo
                    })
                    .ToListAsync();

                response.Successful = true;
                response.Data = roles;
                response.Message = "Roles obtenidos exitosamente";
            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Error al obtener los roles";
                response.Errors.Add(ex.InnerException?.Message ?? ex.Message);
            }

            return response;
        }

    }
}
