using Microsoft.EntityFrameworkCore;
using AgroTrace.Infrastructure.Data;
using AgroTrace.DTO;
using AgroTrace.Helpers;
using AgroTrace.Domain.Entities;



namespace AgroTrace.Service
{
    public class UsuarioService : IUsuario
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _password;
        private readonly ITokenGenerator _refreshToken;
   


        public UsuarioService(
            AppDbContext context,
            PasswordService password,
            ITokenGenerator refreshToken
           )
        {
            _context = context;
            _password = password;
            _refreshToken = refreshToken;
          
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

                if (usuario.Activo)
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
            var response = new Response<AgregarUsuarioResponse>
            {
                Errors = new List<string>()
            };

            try
            {
                var errores = new List<string>();

                if (string.IsNullOrWhiteSpace(usuario.Nombre))
                    errores.Add("El Nombre es obligatorio");

                if (string.IsNullOrWhiteSpace(usuario.Apellido))
                    errores.Add("El Apellido es obligatorio");

                if (string.IsNullOrWhiteSpace(usuario.Email))
                    errores.Add("El Email es obligatorio");

                if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute()
                   .IsValid(usuario.Email))
                {
                    errores.Add("El Email no es válido");
                }

                if (string.IsNullOrWhiteSpace(usuario.Username))
                    errores.Add("El Username es obligatorio");

                if (string.IsNullOrWhiteSpace(usuario.Password))
                    errores.Add("La contraseña es obligatoria");

                if (usuario.Password.Length < 6)
                    errores.Add("La contraseña debe tener al menos 6 caracteres");

                if (errores.Any())
                {
                    response.Successful = false;
                    response.Errors = errores;
                    response.Message = "Errores de validación";
                    return response;
                }

                var existe = await _context.Usuarios
                    .Where(u => u.Username == usuario.Username || u.Email == usuario.Email)
                    .ToListAsync();

                if (existe.Any(u => u.Username == usuario.Username))
                    return ResponseHelper.Fail<AgregarUsuarioResponse>($"El usuario {usuario.Username} ya existe");

                if (existe.Any(u => u.Email == usuario.Email))
                    return ResponseHelper.Fail<AgregarUsuarioResponse>($"El email {usuario.Email} ya existe");

                var rolExiste = await _context.Roles
                    .AnyAsync(r => r.Id == usuario.RolId);

                if (!rolExiste)
                    return ResponseHelper.Fail<AgregarUsuarioResponse>("El rol no existe");

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
            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Error al agregar el usuario";

                if (ex.InnerException != null)
                    response.Errors.Add(ex.InnerException.Message);
                else
                    response.Errors.Add(ex.Message);
            }

            return response;
        }

        public async Task<Response<ActualizarUsuarioResponse>> ActualizarUsuario(int id, ActualizarUsuarioRequest usuario)
        {
            var response = new Response<ActualizarUsuarioResponse>();

            try
            {
                var entity = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (entity == null)
                    return ResponseHelper.Fail<ActualizarUsuarioResponse>($"Usuario con id {id} no encontrado");

               
                if (string.IsNullOrWhiteSpace(usuario.Email))
                    return ResponseHelper.Fail<ActualizarUsuarioResponse>("El email es obligatorio");

                var rolExiste = await _context.Roles
                    .AnyAsync(r => r.Id == usuario.RolId);

                if (!rolExiste)
                    return ResponseHelper.Fail<ActualizarUsuarioResponse>("El rol no existe");

               
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
                    UsuarioModificacion = entity.UsuarioModificacion,
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
              

                if (usuario.Rol == null)
                    throw new Exception("El usuario no tiene rol asignado");

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