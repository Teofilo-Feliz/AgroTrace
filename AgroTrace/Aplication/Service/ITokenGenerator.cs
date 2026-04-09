using AgroTrace.Aplication.DTO;
using AgroTrace.Domain.Entities;

namespace AgroTrace.Aplication.Service
{
    public interface ITokenGenerator
    {
        Task<Response<LoginResponse>> RefreshToken(string refreshToken);
        Task<Response<LoginResponse>> GenerateTokens(Usuario usuario);
    }
}
