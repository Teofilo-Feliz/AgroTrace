using AgroTrace.Aplication.DTO;
using AgroTrace.Domain.Entities;

namespace AgroTrace.Infrastructure.PatronRepository.UsuarioRepository
{
    public interface IUsuarioRepository
    {
        Task<(List<Usuario>, int)> ObtenerUsuarios(Filtro filtro);
        Task<bool> UsernameExiste(string username);
        Task<bool> EmailExiste(string email);
        Task <Usuario?> ObtenerUsuarioPorId(int id);
        Task AgregarUsuario(Usuario usuario);
        Task <Usuario?>ActualizarUsuario(int id);
        Task<Usuario?> LogueoUsuario(string username);
    }
}
