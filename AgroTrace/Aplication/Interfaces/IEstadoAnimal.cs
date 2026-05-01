using AgroTrace.Aplication.DTO;

namespace AgroTrace.Aplication.Interfaces
{
    public interface IEstadoAnimal
    {
        Task<Response<EstadoAnimalResponse>> ObtenerEstadoAnimal(Filtro filtro);
        Task<Response<AgregarEstadoAnimalResponse>> AgregarEstadoAnimal(AgregarEstadoAnimalRequest request);

    }
}
