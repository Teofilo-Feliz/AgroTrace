using AgroTrace.Aplication.DTO;
using AgroTrace.Aplication.Helpers;
using AgroTrace.Aplication.Interfaces;
using AgroTrace.Domain.Entities;
using AgroTrace.Infrastructure.Data;
using AgroTrace.Infrastructure.PatronRepository.GenericRepository;
using AgroTrace.Infrastructure.PatronRepository.UsuarioRepository;
using AgroTrace.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;




namespace AgroTrace.Aplication.Service
{
    public class UsuarioService : IUsuario
    {
        private readonly IRepository<Usuario> _repository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PasswordService _password;
        private readonly ITokenGenerator _refreshToken;
        private readonly IValidationService _validation;

        public UsuarioService(
            IRepository<Usuario> repository,
            IUsuarioRepository usuarioRepository,
            IUnitOfWork unitOfWork,
            PasswordService password,
            ITokenGenerator refreshToken,
            IValidationService validation)
        {
            _repository = repository;
            _usuarioRepository = usuarioRepository;
            _unitOfWork = unitOfWork;
            _password = password;
            _refreshToken = refreshToken;
            _validation = validation;
        }

        public async Task<Response<UsuariosResponse>> ObtenerUsuarios(Filtro filtro)
        {
            var response = new Response<UsuariosResponse>();

            try
            {
                var (usuarios, total) = await _usuarioRepository.ObtenerUsuarios(filtro);

                if (!usuarios.Any())
                {
                    response.Successful = true;
                    response.Message = "No hay usuarios registrados";
                    response.DataList = new List<UsuariosResponse>();
                    response.EntityId = total;
                    return response;
                }

                var data = usuarios.Select(u => new UsuariosResponse
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Apellido = u.Apellido,
                    Username = u.Username,
                    Email = u.Email!,
                    Rol = u.Rol != null ? u.Rol.Nombre : "Sin rol",
                    Activo = u.Activo
                }).ToList();

                response.Successful = true;
                response.Message = "Usuarios obtenidos exitosamente";
                response.DataList = data;
                response.EntityId = total;

                return response;
            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Error al obtener los usuarios";
                response.Errors = new List<string>
        {
            ex.InnerException?.Message ?? ex.Message
        };

                return response;
            }
        }

        public async Task<Response<UsuariosResponse>> ObtenerUsuarioId(int id)
        {
            var response = new Response<UsuariosResponse>();

            try
            {
                var usuario = await _usuarioRepository.ObtenerUsuarioPorId(id);

                if (usuario == null)
                {
                    response.Successful = false;
                    response.Message = $"Usuario con id {id} no encontrado";
                    return response;
                }
                var data = new UsuariosResponse
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Username = usuario.Username,
                    Email = usuario.Email!,
                    Rol = usuario.Rol != null ? usuario.Rol.Nombre : "Sin rol",
                    Activo = usuario.Activo
                };

                response.Successful = true;
                response.Data = data;
                response.Message = $"Usuario {usuario.Username} obtenido exitosamente";

                return response;
            }
            catch (Exception ex)
            {
                return new Response<UsuariosResponse>
                {
                    Successful = false,
                    Message = "Error al obtener el usuario",
                    Errors = new List<string> { ex.Message }
                };
            }
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

                if (await _usuarioRepository.UsernameExiste(usuario.Username))
                    errores.Add($"El usuario {usuario.Username} ya existe");

                if (await _usuarioRepository.EmailExiste(usuario.Email))
                    errores.Add($"El email {usuario.Email} ya existe");

                if (errores.Any())
                {
                    return new Response<AgregarUsuarioResponse>
                    {
                        Successful = false,
                        Message = "Errores de validación",
                        Errors = errores
                    };
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

         
                await _usuarioRepository.AgregarUsuario(entity);

                
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
                    Activo = entity.Activo
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


                var entity = await _usuarioRepository.ActualizarUsuario(id);
                    

                if (entity == null)
                {
                    errores.Add($"Usuario con id {id} no encontrado");
                }


                var emailExiste = await _usuarioRepository.EmailExiste(usuario.Email);
                   

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

                await _unitOfWork.SaveChangesAsync();

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

                var usuario = await _usuarioRepository.LogueoUsuario(username);

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