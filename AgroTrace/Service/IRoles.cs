using AgroTrace.DTO;

namespace AgroTrace.Service
{
    public interface IRoles
    {
        Task<Response<List<RolesResponse>>> ObtenerRoles();
        Task<Response<RolesResponse>> AgregarRol(AgregarRolRequest rol);
        Task<Response<RolesResponse>> ActualizarRol(int id, AgregarRolRequest rol);
    }
}
