using AgroTrace.Aplication.DTO;
using AgroTrace.Domain.Entities;

namespace AgroTrace.Infrastructure.PatronRepository.RolesRepository
{
    public interface IRolesRepository
    {
        Task<(List<Rol>, int)> ObtenerRoles(Filtro filtro);
        Task AgregarRol(Rol roles);
        
        Task<bool> ExisteRol(string nombre);
        Task<bool> ExisteRolId(string nombre, int id);
        Task<Rol?> ValidarPorId(int id);
        Task<Rol?> ObtenerRolId(int id);




    }
}
