namespace AgroTrace.Domain.Entities
{
    public class Finca: BaseEntity
    {
        public string Nombre { get; set; }

        public string Ubicacion { get; set; }

        public decimal Tamaño { get; set; }

        public int UsuarioPropietarioId { get; set; }
        public Usuario UsuarioPropietario { get; set; }

        public ICollection<Animal> Animales { get; set; } = new List<Animal>();
        public ICollection<Produccion> Producciones { get; set; } = new List<Produccion>();
        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
    }
}
