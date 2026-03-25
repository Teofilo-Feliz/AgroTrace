namespace AgroTrace.Domain.Entities
{
    public class TipoProduccion: BaseEntity
    {
        public string Nombre { get; set; }
        public ICollection<Produccion> Producciones { get; set; } = new List<Produccion>();


    }
}
