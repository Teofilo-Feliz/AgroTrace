namespace AgroTrace.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaModificacion {  get; set; } 
        public string UsuarioCreacion { get; set; }
        public string? UsuarioModificacion { get; set;}
        public bool Activo { get; set; } = true;
    }
}
