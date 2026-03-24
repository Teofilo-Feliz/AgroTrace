namespace AgroTrace.Domain.Entities
{
    public class EstadoAnimal: BaseEntity
    {
        public string Nombre { get; set; }

        public ICollection<Animal> Animales { get; set; }
    }
}
