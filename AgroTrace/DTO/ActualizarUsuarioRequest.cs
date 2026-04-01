namespace AgroTrace.DTO
{
    public class ActualizarUsuarioRequest
    {

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public int RolId { get; set; }
        public bool Activo { get; set; }
    }
}
