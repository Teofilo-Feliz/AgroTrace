namespace AgroTrace.Domain.Entities
{
    public class TipoGasto : BaseEntity
    {
        public string Nombre { get; set; }
        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

    }
}
