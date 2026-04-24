using AgroTrace.Aplication.DTO;
using AgroTrace.Aplication.Interfaces;
using AgroTrace.Domain.Entities;
using AgroTrace.Infrastructure.Data;
using AgroTrace.Infrastructure.PatronRepository.RolesRepository;
using AgroTrace.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AgroTrace.Aplication.Service
{
    public class RolesServices: IRoles     
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly IUnitOfWork _unitOfWork;
        public RolesServices(IRolesRepository rolesRepository, IUnitOfWork unitOfWork)
        {
            _rolesRepository = rolesRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<RolesResponse>> ObtenerRoles(Filtro filtro)
        {
            var response = new Response<RolesResponse>
            {
                Errors = new List<string>()
            };

            try
            {
               
                filtro.PageNumber = filtro.PageNumber <= 0 ? 1 : filtro.PageNumber;
                filtro.PageSize = filtro.PageSize <= 0 ? 10 : filtro.PageSize;

                var (roles, total) = await _rolesRepository.ObtenerRoles(filtro);

               
                if (!roles.Any())
                {
                    response.Successful = true;
                    response.Message = "No hay roles registrados";
                    response.DataList = new List<RolesResponse>();
                    response.EntityId = total;
                    return response;
                }

               
                var data = roles.Select(r => new RolesResponse
                {
                    Id = r.Id,
                    Nombre = r.Nombre,
                    Descripcion = r.Descripcion,
                    Activo = r.Activo
                }).ToList();


                response.Successful = true;
                response.Message = "Roles obtenidos exitosamente";
                response.DataList = data;
                response.EntityId = total;

                return response;
            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Error al obtener los roles";
                response.Errors.Add(ex.InnerException?.Message ?? ex.Message);

                return response;
            }
        }


        public async Task<Response<RolesResponse>> AgregarRol(AgregarRolRequest rol)
        {
            var response = new Response<RolesResponse>();

            try
            {

                if (string.IsNullOrWhiteSpace(rol.Nombre))
                {
                    return new Response<RolesResponse>
                    {
                        Successful = false,
                        Message = "El nombre del rol es requerido"
                    };
                }
                    var nombre = rol.Nombre.Trim();

                var existe = await _rolesRepository.ExisteRol(nombre);  


                if (existe)
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

                await _rolesRepository.AgregarRol(nuevoRol);
                await _unitOfWork.SaveChangesAsync();

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
                
                if (string.IsNullOrWhiteSpace(rol.Nombre))
                {
                    response.Successful = false;
                    response.Message = "El nombre del rol no puede estar vacío";
                    return response;
                }

                var rolExistente = await _rolesRepository.ObtenerRolId(id);
                   
                if (rolExistente == null)
                {
                    response.Successful = false;
                    response.Message = "El rol no existe";
                    return response;
                }

               
                var nombre = rol.Nombre.Trim();

                var existe = await _rolesRepository.ExisteRolId(nombre, id);
                   

                if (existe)
                {
                    response.Successful = false;
                    response.Message = "Ya existe un rol con ese nombre";
                    return response;
                }

            
                rolExistente.Nombre = rol.Nombre.Trim();
                rolExistente.Descripcion = rol.Descripcion?.Trim();

                await _unitOfWork.SaveChangesAsync(); 

                response.Successful = true;
                response.Message = "Rol actualizado exitosamente";
                response.Data = new RolesResponse
                {
                    Id = rolExistente.Id,
                    Nombre = rolExistente.Nombre,
                    Descripcion = rolExistente.Descripcion,
                    Activo = rolExistente.Activo
                };

                return response;
            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Error al actualizar el rol";
                response.Errors.Add(ex.InnerException?.Message ?? ex.Message);
                return response;
            }
        }
    }
}
