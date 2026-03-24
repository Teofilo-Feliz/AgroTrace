namespace AgroTrace.Domain.Entities
{
    public class Tratamiento : BaseEntity
    {
        public int AnimalId { get; set; }
        public Animal Animal { get; set; }

        public DateTime Fecha { get; set; }

        public string Diagnostico { get; set; }

        public string Medicamento { get; set; }

        public decimal Costo { get; set; }
    }
}
