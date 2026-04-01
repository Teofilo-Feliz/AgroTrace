using AgroTrace.DTO;

namespace AgroTrace.Service
{
    public interface IRoles
    {
        Task<Response<List<RolesResponse>>> ObtenerRoles();
    }
}
