using AgroTrace.Aplication.DTO;

namespace AgroTrace.Aplication.Interfaces
{
    public interface IAnimal
    {
        Task<Response<AgregarAnimalesResponse>> AgregarAnimal(AgregarAnimalesRequest animal);

    }
}
