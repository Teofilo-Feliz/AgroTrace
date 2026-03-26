using Microsoft.EntityFrameworkCore;
using AgroTrace.Infrastructure.Data;
using AgroTrace.DTO;
using AgroTrace.Helpers;


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