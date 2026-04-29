namespace AgroTrace.Aplication.DTO
{
    public class ActualizarFincaResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public decimal Tamaño { get; set; }
        public int UsuarioPropietarioId { get; set; }
        public string NombrePropietario { get; set; } = null!;
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool Activo { get; set; }


    }
}
