namespace AgroTrace.Domain.Entities
{
    public class Animal: BaseEntity
    {
        public string Codigo { get; set; }

        public string Nombre { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string Sexo { get; set; }

        public decimal Peso { get; set; }

        public int FincaId { get; set; }
        public Finca Finca { get; set; }

        public int TipoAnimalId { get; set; }
        public TipoAnimal TipoAnimal { get; set; }

        public int RazaId { get; set; }
        public Raza Raza { get; set; }

        public int EstadoAnimalId { get; set; }
        public EstadoAnimal EstadoAnimal { get; set; }

        public ICollection<ProduccionDetalle> ProduccionDetalles { get; set; } = new List<ProduccionDetalle>();
        public ICollection<Tratamiento> Tratamientos { get; set; } = new List<Tratamiento>();

    }
}
