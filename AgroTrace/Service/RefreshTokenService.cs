using AgroTrace.Domain.Entities;
using AgroTrace.DTO;
using AgroTrace.Helpers;
using AgroTrace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgroTrace.Service
{
    public class RefreshTokenService: ITokenGenerator
    {
        private readonly AppDbContext _context;
        private readonly JWTService _jwt;
        private readonly IConfiguration _configuration;

        public RefreshTokenService(AppDbContext context, JWTService jwt, IConfiguration configuration)
        {
            _context = context;
            _jwt = jwt;
            _configuration = configuration;
        }

        public async Task<Response<LoginResponse>> GenerateTokens(Usuario usuario)
        {
            var response = new Response<LoginResponse>();

            var token = _jwt.GenerateToken(usuario);
            var refreshToken = _jwt.GenerateRefreshToken();

            var refreshEntity = new RefreshToken
            {
                Token = refreshToken,
                Expiration = DateTime.UtcNow.AddHours(
                    int.Parse(_configuration["Jwt:RefreshTokenDuracionHoras"]!)
                ),
                CreatedAt = DateTime.UtcNow,
                UsuarioId = usuario.Id,
                IsRevoked = false
            };

            _context.RefreshTokens.Add(refreshEntity);
            await _context.SaveChangesAsync();

            var fechaGeneracion = DateTime.UtcNow;
            var duracion = int.Parse(_configuration["Jwt:DuracionEnMinutos"]!);

            response.Successful = true;
            response.Data = new LoginResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                FechaGeneracion = fechaGeneracion,
                DuracionEnMinutos = duracion,
                FechaExpiracion = fechaGeneracion.AddMinutes(duracion)
            };

            return response;
        }

        public async Task<Response<LoginResponse>> RefreshToken(string refreshToken)
        {
            var response = new Response<LoginResponse>();

            try
            {
                var storedToken = await _context.RefreshTokens
                    .Include(r => r.Usuario)
                    .ThenInclude(u => u.Rol)
                    .FirstOrDefaultAsync(r => r.Token == refreshToken);

                if (storedToken == null ||
                    storedToken.IsRevoked ||
                    storedToken.Expiration < DateTime.UtcNow)
                {
                    response.Successful = false;
                    response.Message = "Refresh token inválido";
                    return response;
                }

              
                storedToken.IsRevoked = true;

              
                var newRefreshToken = _jwt.GenerateRefreshToken();

                var refreshDuration = int.Parse(
                    _configuration["Jwt:RefreshTokenDuracionHoras"]!
                );

                var newRefreshEntity = new RefreshToken
                {
                    Token = newRefreshToken,
                    Expiration = DateTime.UtcNow.AddHours(refreshDuration),
                    CreatedAt = DateTime.UtcNow,
                    UsuarioId = storedToken.UsuarioId,
                    IsRevoked = false
                };

                _context.RefreshTokens.Add(newRefreshEntity);

              
                var newJwt = _jwt.GenerateToken(storedToken.Usuario);

                await _context.SaveChangesAsync();

                var fechaGeneracion = DateTime.UtcNow;
                var duracion = int.Parse(_configuration["Jwt:DuracionEnMinutos"]!);

                response.Successful = true;
                response.Data = new LoginResponse
                {
                    Token = newJwt,
                    RefreshToken = newRefreshToken,
                    FechaGeneracion = fechaGeneracion,
                    DuracionEnMinutos = duracion,
                    FechaExpiracion = fechaGeneracion.AddMinutes(duracion)
                };

                return response;
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
