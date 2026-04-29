using AgroTrace.Aplication.DTO;

namespace AgroTrace.Aplication.Interfaces
{
    public interface IFinca
    {
        Task<Response<FincasResponse>> ObtenerFincas(Filtro filtro);
        Task<Response<AgregarFincaResponse>> AgregarFinca(AgregarFincaRequest finca);
        Task<Response<ActualizarFincaResponse>> ActualizarFinca(ActualizarFincaRequest finca, int id);


    }
}
