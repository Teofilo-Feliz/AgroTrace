namespace AgroTrace.DTO
{
    public class AgregarUsuarioRequest
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int RolId {  get; set; }
       

    }
}
