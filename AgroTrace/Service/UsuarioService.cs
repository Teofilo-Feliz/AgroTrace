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
            ITokenGenerator refreshToken)
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

                if (usuario.Activo.HasValue)
                {
                    query = query.Where(u => u.Activo == usuario.Activo.Value);
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
                    .Take(usuario.PageSize = 10)
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

                if (string.IsNullOrWhiteSpace(usuario.PasswordHash))
                    errores.Add("La contraseña es obligatoria");

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
                    PasswordHash = usuario.PasswordHash,
                    RolId = usuario.RolId,
                    FechaCreacion = DateTime.UtcNow,
                    Activo = true
                };

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
                response.Errors.Add(ex.Message);
            }

            return response;
        }








































        public async Task<Response<LoginResponse>> LoguearUsuario(string username, string password)
        {
            var response = new Response<LoginResponse>();

            try
            {
                var usuario = await _context.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (usuario == null)
                {
                    response.Successful = false;
                    response.Message = "Usuario o contraseña incorrectos";
                    return response;
                }

                var isValid = _password.VerifyPassword(usuario, password);

                if (!isValid)
                {
                    response.Successful = false;
                    response.Message = "Usuario o contraseña incorrectos";
                    return response;
                }

                
                var tokenResponse = await _refreshToken.GenerateTokens(usuario);

                return tokenResponse;
            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}