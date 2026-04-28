using AgroTrace.Aplication.DTO;

namespace AgroTrace.Aplication.Interfaces
{
    public interface IFinca
    {
        Task<Response<AgregarFincaResponse>> AgregarFinca(AgregarFincaRequest finca);


    }
}
