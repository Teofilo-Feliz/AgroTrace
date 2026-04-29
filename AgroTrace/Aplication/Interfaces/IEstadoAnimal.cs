using AgroTrace.Aplication.DTO;

namespace AgroTrace.Aplication.Interfaces
{
    public interface IEstadoAnimal
    {
        Task<Response<AgregarEstadoAnimalResponse>> AgregarEstadoAnimal(AgregarEstadoAnimalRequest request);

    }
}
