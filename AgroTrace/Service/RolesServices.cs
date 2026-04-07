


using AgroTrace.Domain.Entities;
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


        public async Task<Response<RolesResponse>> AgregarRol(AgregarRolRequest rol)
        {
            var response = new Response<RolesResponse>();

            try
            {
                var nombre = rol.Nombre.Trim().ToLower();

                var existe = await _context.Roles
                    .FirstOrDefaultAsync(r => r.Nombre.ToLower() == nombre);

                if (existe != null)
                {
                    response.Successful = false;
                    response.Message = "El rol ya existe";
                    return response;
                }

                var nuevoRol = new Rol
                {
                    Nombre = rol.Nombre.Trim(),
                    Activo = true
                };

                _context.Roles.Add(nuevoRol);
                await _context.SaveChangesAsync();

                response.Successful = true;
                response.Message = "Rol creado exitosamente";
                response.Data = new RolesResponse
                {
                    Id = nuevoRol.Id,
                    Nombre = nuevoRol.Nombre,
                    Descripcion = nuevoRol.Descripcion,
                    Activo = nuevoRol.Activo
                };

                return response;
            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Error al crear el rol";
                response.Errors.Add(ex.InnerException?.Message ?? ex.Message);
                return response;
            }
        }

        public async Task<Response<RolesResponse>> ActualizarRol(int id, AgregarRolRequest rol)
        {
            var response = new Response<RolesResponse>();
            try
            {
                var rolExistente = _context.Roles.FirstOrDefault(r => r.Id == id);
                if (rolExistente == null)
                {
                    response.Successful = false;
                    response.Message = "El rol no existe";
                    
                }

               rolExistente.Nombre = rol.Nombre.Trim();
               rolExistente.Descripcion = rol.Descripcion.Trim();
                await _context.SaveChangesAsync();

                response.Successful = true;
                response.Message = "Rol actualizado exitosamente";
                response.Data = new RolesResponse
                {
                    Id = rolExistente.Id,
                    Nombre = rolExistente.Nombre,
                    Descripcion = rolExistente.Descripcion,
                    Activo = rolExistente.Activo
                };






            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Error al actualizar el rol";
                response.Errors.Add(ex.InnerException?.Message ?? ex.Message);


            }
            return response;

        } 
    }
}
