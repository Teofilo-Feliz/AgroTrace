using AgroTrace.Aplication.DTO;

namespace AgroTrace.Aplication.Service
{
    public interface IUsuario
    {
        Task<Response<UsuariosResponse>> ObtenerUsuarios(UsuarioFiltro usuario);
        Task<Response<UsuariosResponse>> ObtenerUsuariosId(int id);
        Task<Response<AgregarUsuarioResponse>> AgregarUsuario(AgregarUsuarioRequest usuario);
        Task<Response<ActualizarUsuarioResponse>> ActualizarUsuario(int id, ActualizarUsuarioRequest usuario);
        Task<Response<LoginResponse>> LoguearUsuario(string username, string password);
    }
}
