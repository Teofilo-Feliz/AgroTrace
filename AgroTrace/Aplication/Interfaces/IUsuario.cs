using AgroTrace.Aplication.DTO;

namespace AgroTrace.Aplication.Interfaces
{
    public interface IUsuario
    {
        Task<Response<UsuariosResponse>> ObtenerUsuarios(Filtro filtro);
        Task<Response<UsuariosResponse>> ObtenerUsuarioId(int id);
        Task<Response<AgregarUsuarioResponse>> AgregarUsuario(AgregarUsuarioRequest usuario);
        Task<Response<ActualizarUsuarioResponse>> ActualizarUsuario(int id, ActualizarUsuarioRequest usuario);
        Task<Response<LoginResponse>> LoguearUsuario(string username, string password);
    }
}
