namespace AgroTrace.Domain.Entities
{
    public class TipoAnimal: BaseEntity
    {
        public string Nombre { get; set; }

        public ICollection<Raza> Razas { get; set; } = new List<Raza>();
        public ICollection<Animal> Animales { get; set; } = new List<Animal>();
    }
}
