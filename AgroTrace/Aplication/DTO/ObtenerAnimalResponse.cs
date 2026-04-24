namespace AgroTrace.Aplication.DTO
{
    public class ObtenerAnimalResponse
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; } = null!;
        public decimal? Peso { get; set; }

        public int? FincaId { get; set; }
        public string FincaNombre { get; set; } = null!;

        public int? TipoAnimalId { get; set; }
        public string TipoAnimalNombre { get; set; } = null!;

        public int? RazaId { get; set; }
        public string RazaNombre { get; set; } = null!;

        public int? EstadoAnimalId { get; set; }
        public string EstadoAnimalNombre { get; set; } = null!;

        public bool? Activo { get; set; }

    }
}
