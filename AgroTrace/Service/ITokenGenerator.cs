using AgroTrace.Domain.Entities;
using AgroTrace.DTO;

namespace AgroTrace.Service
{
    public interface ITokenGenerator
    {
        Task<Response<LoginResponse>> RefreshToken(string refreshToken);
        Task<Response<LoginResponse>> GenerateTokens(Usuario usuario);
    }
}
