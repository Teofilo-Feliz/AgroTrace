using AgroTrace.Aplication.DTO;
using AgroTrace.Aplication.Helpers;
using AgroTrace.Aplication.Validators;
using AgroTrace.Domain.Entities;
using AgroTrace.Infrastructure.Data;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;



namespace AgroTrace.Aplication.Service
{
    public class UsuarioService : IUsuario
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _password;
        private readonly ITokenGenerator _refreshToken;
        private readonly IValidationService _validation;


        public UsuarioService(
            AppDbContext context,
            PasswordService password,
            ITokenGenerator refreshToken,
            IValidationService validation)
        {
            _context = context;
            _password = password;
            _refreshToken = refreshToken;
            _validation = validation;
        }

        public async Task<Response<UsuariosResponse>> ObtenerUsuarios(UsuarioFiltro usuario)
        {
            var response = new Response<UsuariosResponse>();
            try
            {
                var query = _context.Usuarios.AsQueryable();

                if (!string.IsNullOrWhiteSpace(usuario.Buscar))
                {
                    query = query.Where(u =>
                    u.Nombre.Contains(usuario.Buscar) ||
                    u.Username.Contains(usuario.Buscar));
                }

                if (usuario.Activo != null)
                {
                    query = query.Where(u => u.Activo == usuario.Activo);
                }

                query = usuario.OrdenarPor?.ToLower() switch
                {
                    "nombre" => usuario.Descendente
                 ? query.OrderByDescending(u => u.Nombre)
                 : query.OrderBy(u => u.Nombre),

                    "username" => usuario.Descendente
                        ? query.OrderByDescending(u => u.Username)
                        : query.OrderBy(u => u.Username),

                    _ => query.OrderBy(u => u.Id)
                };

                var totalRegistros = await query.CountAsync();

                var usuarios = await query
                    .Skip((usuario.PageNumber -1)* usuario.PageSize)
                    .Take(usuario.PageSize)
                    .Select(u => new UsuariosResponse
                    {
                        Id = u.Id,
                        Nombre = u.Nombre,
                        Apellido = u.Apellido,
                        Username = u.Username,
                        Email = u.Email!,
                        Rol = u.Rol != null ? u.Rol.Nombre : "Sin rol",
                        Activo = u.Activo,


                    })
                       .ToListAsync();


                response.Successful = true;
                response.DataList = usuarios;
                response.Message = usuarios.Any()
                    ? "Usuarios Obtenidos Exitosamente"
                    : "No hay usuarios registrados";
                response.EntityId = totalRegistros;


            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Errors.Add($"Error al obtener los usuarios {ex.Message}");

                
            }
            return response;


        }

        public async Task<Response<UsuariosResponse>> ObtenerUsuariosId(int id)
        {
            var response = new Response<UsuariosResponse>();

            try
            {
             var usuario = await _context.Usuarios.AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new UsuariosResponse
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Username = u.Username,
                Email = u.Email!,
                Rol = u.Rol != null ? u.Rol.Nombre : "Sin rol",
                Activo = u.Activo
            })
            .FirstOrDefaultAsync();

                if (usuario == null)
                {
                    response.Successful = false;
                    response.Message = $"Usuario con este id {id} no se ha encontrado";
                    return response;
                }

                response.Successful = true;
                response.Data = usuario;
                response.Message = $"Usuario {usuario.Username} obtenido exitosamente";

                return response;
            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Error al obtener el usuario";
               
                if (ex.InnerException != null)
                    response.Errors.Add(ex.InnerException.Message);
                else
                    response.Errors.Add(ex.Message);

            }
            return response;
        }

        public async Task<Response<AgregarUsuarioResponse>> AgregarUsuario(AgregarUsuarioRequest usuario)
        {
            var response = new Response<AgregarUsuarioResponse>();
            var errores = new List<string>();

            try
            {
               
                try
                {
                    await _validation.ValidateAsync(usuario);
                }
                catch (FluentValidation.ValidationException ex)
                {
                    errores.AddRange(ex.Errors.Select(e => e.ErrorMessage));
                }

               
                var existe = await _context.Usuarios
                    .Where(u => u.Username == usuario.Username || u.Email == usuario.Email)
                    .ToListAsync();

                if (existe.Any(u => u.Username == usuario.Username))
                {
                    errores.Add($"El usuario {usuario.Username} ya existe");
                }

                if (existe.Any(u => u.Email == usuario.Email))
                {
                    errores.Add($"El email {usuario.Email} ya existe");
                }

              
                if (errores.Any())
                {
                    response.Successful = false;
                    response.Message = "Errores de validación";
                    response.Errors = errores;
                    return response;
                }

               
                var entity = new Usuario
                {
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Username = usuario.Username,
                    Email = usuario.Email,
                    RolId = usuario.RolId,
                    Activo = true
                };

                entity.PasswordHash = _password.HashPassword(entity, usuario.Password);

                _context.Usuarios.Add(entity);
                await _context.SaveChangesAsync();

                response.Successful = true;
                response.Message = "Usuario agregado exitosamente";
                response.Data = new AgregarUsuarioResponse
                {
                    Id = entity.Id,
                    Nombre = entity.Nombre,
                    Apellido = entity.Apellido,
                    Username = entity.Username,
                    Email = entity.Email,
                    RolId = entity.RolId,
                    FechaCreacion = entity.FechaCreacion,
                    Activo = entity.Activo,
                };

                return response;
            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Error al agregar el usuario";
                response.Errors.Add(ex.InnerException?.Message ?? ex.Message);

                return response;
            }
        }

        public async Task<Response<ActualizarUsuarioResponse>> ActualizarUsuario(int id, ActualizarUsuarioRequest usuario)
        {
            var response = new Response<ActualizarUsuarioResponse>();
            var errores = new List<string>();

            try
            {
                
                try
                {
                    await _validation.ValidateAsync(usuario);
                }
                catch (FluentValidation.ValidationException ex)
                {
                    errores.AddRange(ex.Errors.Select(e => e.ErrorMessage));
                }

             
                var entity = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (entity == null)
                {
                    errores.Add($"Usuario con id {id} no encontrado");
                }

             
                var emailExiste = await _context.Usuarios
                    .AnyAsync(u => u.Email == usuario.Email && u.Id != id);

                if (emailExiste)
                {
                    errores.Add($"El email {usuario.Email} ya está en uso");
                }

             
                if (errores.Any())
                {
                    response.Successful = false;
                    response.Message = "Errores de validación";
                    response.Errors = errores;
                    return response;
                }

               
                entity.Nombre = usuario.Nombre;
                entity.Apellido = usuario.Apellido;
                entity.Email = usuario.Email;
                entity.RolId = usuario.RolId;
                entity.Activo = usuario.Activo;

                await _context.SaveChangesAsync();

                response.Successful = true;
                response.Message = "Usuario actualizado exitosamente";
                response.Data = new ActualizarUsuarioResponse
                {
                    Nombre = entity.Nombre,
                    Apellido = entity.Apellido,
                    Email = entity.Email,
                    RolId = entity.RolId,
                    FechaModificacion = entity.FechaModificacion!.Value,
                    UsuarioModificacion = entity.UsuarioModificacion!,
                    Activo = entity.Activo
                };

                return response;
            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Error al actualizar el usuario";
                response.Errors.Add(ex.InnerException?.Message ?? ex.Message);
                return response;
            }
        }

        public async Task<Response<LoginResponse>> LoguearUsuario(string username, string password)
        {
            var response = new Response<LoginResponse>();

            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                    return ResponseHelper.Fail<LoginResponse>("Username y password son requeridos");

                var usuario = await _context.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());

                if (usuario == null)
                    return ResponseHelper.Fail<LoginResponse>("Usuario o contraseña incorrectos");

                if (!usuario.Activo)
                    return ResponseHelper.Fail<LoginResponse>("Usuario inactivo");

                if (usuario.Rol == null)
                    return ResponseHelper.Fail<LoginResponse>("Usuario sin rol asignado");

                var isValid = _password.VerifyPassword(usuario, password);

                if (!isValid)
                    return ResponseHelper.Fail<LoginResponse>("Usuario o contraseña incorrectos");

                var tokenResponse = await _refreshToken.GenerateTokens(usuario);

                if (!tokenResponse.Successful || tokenResponse.Data == null)
                    return ResponseHelper.Fail<LoginResponse>("Error generando el token");

                return tokenResponse;
            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Error al iniciar sesión";
                response.Errors.Add(ex.InnerException?.Message ?? ex.Message);
            }

            return response;
        }
    }
}