using AgroTrace.Aplication.DTO;

namespace AgroTrace.Aplication.Interfaces
{
    public interface IAnimal
    {
        Task<Response<AgregarAnimalesResponse>> AgregarAnimal(AgregarAnimalesRequest animal);
        Task<Response<ObtenerAnimalResponse>> ObtenerAnimales(FiltroAnimal filtro);
        Task<Response<ActualizarAnimalResponse>> ActualizarAnimal(int id,ActualizarAnimalRequest animal);

    }
}
