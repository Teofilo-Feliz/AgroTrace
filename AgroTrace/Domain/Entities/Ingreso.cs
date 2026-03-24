namespace AgroTrace.Domain.Entities
{
    public class Ingreso: BaseEntity
    {
        public DateTime Fecha { get; set; }

        public decimal Monto { get; set; }

        public int FincaId { get; set; }
        public Finca Finca { get; set; }

        public string? Descripcion { get; set; }
    }
}
