using AgroTrace.Aplication.DTO;
using AgroTrace.Domain.Entities;

namespace AgroTrace.Infrastructure.PatronRepository.AnimalRepository
{
    public interface IAnimalRepository
    {
        Task<(List<Animal>, int)> ObtenerAnimales(FiltroAnimal filtro);
        Task<bool> ExisteCodigo(string codigo, int fincaId);
        Task AgregarAnimal (Animal animal);
    }
}
