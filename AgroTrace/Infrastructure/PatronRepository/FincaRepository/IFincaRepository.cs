using AgroTrace.Domain.Entities;

namespace AgroTrace.Infrastructure.PatronRepository.FincaRepository
{
    public interface IFincaRepository
    {
        Task AgregarFinca(Finca finca);
        Task <bool> ExisteUsuario(int usuarioPropiedadId);
        Task <string?> ObtenerNombreUsuarioPropiedad(int usuarioId);
        Task<bool> FincaExiste(string nombre, int usuarioId);
    }
}
