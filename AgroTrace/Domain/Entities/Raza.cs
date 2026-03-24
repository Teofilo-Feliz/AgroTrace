namespace AgroTrace.Domain.Entities
{
    public class Raza: BaseEntity
    {
        public string Nombre { get; set; }

        public int TipoAnimalId { get; set; }
        public TipoAnimal TipoAnimal { get; set; }

        public ICollection<Animal> Animales { get; set; } = new List<Animal>();
    }
}
