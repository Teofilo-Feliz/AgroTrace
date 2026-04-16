using AgroTrace.Aplication.DTO;
using AgroTrace.Aplication.Helpers;
using AgroTrace.Aplication.Interfaces;
using AgroTrace.Aplication.Options;
using AgroTrace.Domain.Entities;
using AgroTrace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AgroTrace.Aplication.Service
{
    public class RefreshTokenService: ITokenGenerator
    {
        private readonly AppDbContext _context;
        private readonly JWTService _jwt;
        private readonly IConfiguration _configuration;
        private readonly JwtOptions _jwtOptions;

        public RefreshTokenService(AppDbContext context, JWTService jwt, IOptions<JwtOptions> options, IConfiguration configuration)
        {
            _context = context;
            _jwt = jwt;
            _configuration = configuration;
            _jwtOptions = options.Value;
        }

        public async Task<Response<LoginResponse>> GenerateTokens(Usuario usuario)
        {
            var response = new Response<LoginResponse>();

            try
            {
                var token = _jwt.GenerateToken(usuario);
                var refreshToken = _jwt.GenerateRefreshToken();

                //var refreshHours = int.TryParse(_configuration["Jwt:RefreshTokenDuracionHoras"], out var h)
                //    ? h
                //    : 24;

                var refreshEntity = new RefreshToken
                {
                    Token = refreshToken,
                    Expiration = DateTime.UtcNow.AddHours(_jwtOptions.DuracionEnHoras),
                    CreatedAt = DateTime.UtcNow,
                    UsuarioId = usuario.Id,
                    IsRevoked = false
                };

                _context.RefreshTokens.Add(refreshEntity);
                await _context.SaveChangesAsync();

                //var duracion = int.TryParse(_configuration["Jwt:DuracionEnMinutos"], out var d)
                //    ? d
                //    : 60;

                var fechaGeneracion = DateTime.UtcNow;

                response.Successful = true;
                response.Data = new LoginResponse
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    FechaGeneracion = fechaGeneracion,
                    DuracionEnMinutos = _jwtOptions.DuracionEnMinutos,
                    FechaExpiracion = fechaGeneracion.AddMinutes(_jwtOptions.DuracionEnMinutos)
                };
            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Error generando tokens";
                response.Errors.Add(ex.InnerException?.Message ?? ex.Message);
            }

            return response;
        }

        public async Task<Response<LoginResponse>> RefreshToken(string refreshToken)
        {
            var response = new Response<LoginResponse>();

            try
            {
                if (string.IsNullOrWhiteSpace(refreshToken))
                    return ResponseHelper.Fail<LoginResponse>("Refresh token vacío");

                refreshToken = refreshToken.Trim();

                var storedToken = await _context.RefreshTokens
                    .Include(r => r.Usuario)
                    .ThenInclude(u => u.Rol)
                    .FirstOrDefaultAsync(r => r.Token == refreshToken);

                if (storedToken == null)
                    return ResponseHelper.Fail<LoginResponse>("Refresh token inválido");

                if (storedToken.IsRevoked || storedToken.Expiration < DateTime.UtcNow)
                    return ResponseHelper.Fail<LoginResponse>("Refresh token inválido o expirado");

                if (storedToken.Usuario == null)
                    return ResponseHelper.Fail<LoginResponse>("Token sin usuario asociado");

                if (storedToken.Usuario.Rol == null)
                    return ResponseHelper.Fail<LoginResponse>("Usuario sin rol");

                storedToken.IsRevoked = true;

                var newRefreshToken = _jwt.GenerateRefreshToken();

                //var refreshDuration = int.TryParse(_configuration["Jwt:RefreshTokenDuracionHoras"], out var h)
                //    ? h
                //    : 24;

                var newRefreshEntity = new RefreshToken
                {
                    Token = newRefreshToken,
                    Expiration = DateTime.UtcNow.AddHours(_jwtOptions.DuracionEnHoras),
                    CreatedAt = DateTime.UtcNow,
                    UsuarioId = storedToken.UsuarioId,
                    IsRevoked = false
                };

                _context.RefreshTokens.Add(newRefreshEntity);

                var newJwt = _jwt.GenerateToken(storedToken.Usuario);

                await _context.SaveChangesAsync();

                //var duracion = int.TryParse(_configuration["Jwt:DuracionEnMinutos"], out var d)
                //    ? d
                //    : 60;

                var fechaGeneracion = DateTime.UtcNow;

                response.Successful = true;
                response.Data = new LoginResponse
                {
                    Token = newJwt,
                    RefreshToken = newRefreshToken,
                    FechaGeneracion = fechaGeneracion,
                    DuracionEnMinutos = _jwtOptions.DuracionEnMinutos,
                    FechaExpiracion = fechaGeneracion.AddMinutes(_jwtOptions.DuracionEnMinutos)
                };

                return response;
            }
            catch (Exception ex)
            {
                response.Successful = false;
                response.Message = "Error al refrescar token";
                response.Errors.Add(ex.InnerException?.Message ?? ex.Message);
                return response;
            }
        }
    }
 }


