using AgroTrace.Aplication.DTO;
using AgroTrace.Domain.Entities;

namespace AgroTrace.Infrastructure.PatronRepository.FincaRepository
{
    public interface IFincaRepository
    {
        Task<(List<Finca>, int)> ObtenerFincas(Filtro filtro);
        Task AgregarFinca(Finca finca);
        Task <bool> ExisteUsuario(int usuarioPropiedadId);
        Task <string?> ObtenerNombreUsuarioPropiedad(int usuarioId);
        Task<bool> FincaExiste(string nombre, int usuarioId);
    }
}
