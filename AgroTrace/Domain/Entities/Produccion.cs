namespace AgroTrace.Domain.Entities
{
    public class Produccion : BaseEntity
    {
        public DateTime Fecha { get; set; }

        public int TipoProduccionId { get; set; }
        public TipoProduccion TipoProduccion { get; set; }

        public int FincaId { get; set; }
        public Finca Finca { get; set; }

        public ICollection<ProduccionDetalle> Detalles { get; set; } = new List<ProduccionDetalle>();
    }
}
