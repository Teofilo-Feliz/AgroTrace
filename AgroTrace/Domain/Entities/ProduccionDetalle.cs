namespace AgroTrace.Domain.Entities
{
    public class ProduccionDetalle : BaseEntity
    {
        public int ProduccionId { get; set; }
        public Produccion Produccion { get; set; }

        public int AnimalId { get; set; }
        public Animal Animal { get; set; }

        public decimal Cantidad { get; set; }
    }
}
