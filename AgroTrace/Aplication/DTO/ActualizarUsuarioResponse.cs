namespace AgroTrace.Aplication.DTO
{
    public class ActualizarUsuarioResponse
    {
      
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public int RolId { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool Activo { get; set; }
    }
}
