using AgroTrace.Aplication.DTO;

namespace AgroTrace.Aplication.Interfaces
{
    public interface IRoles
    {
        Task<Response<RolesResponse>> ObtenerRoles(Filtro filtro);
        Task<Response<RolesResponse>> AgregarRol(AgregarRolRequest rol);
        Task<Response<RolesResponse>> ActualizarRol(int id, AgregarRolRequest rol);
        
    }
}
