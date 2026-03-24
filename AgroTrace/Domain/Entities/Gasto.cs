namespace AgroTrace.Domain.Entities
{
    public class Gasto : BaseEntity
    {
        public DateTime Fecha { get; set; }

        public decimal Monto { get; set; }

        public int TipoGastoId { get; set; }
        public TipoGasto TipoGasto { get; set; }

        public int FincaId { get; set; }
        public Finca Finca { get; set; }

        public string? Descripcion { get; set; }
    }
}
