namespace AgroTrace.Aplication.DTO
{
    public class AgregarUsuarioRequest
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RolId {  get; set; }
       

    }
}
