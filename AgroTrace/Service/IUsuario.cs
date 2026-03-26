using AgroTrace.DTO;

namespace AgroTrace.Service
{
    public interface IUsuario
    {
        Task<Response<LoginResponse>> LoguearUsuario (string username, string password);
    }
}
