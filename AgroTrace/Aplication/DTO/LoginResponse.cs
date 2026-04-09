namespace AgroTrace.Aplication.DTO
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public int DuracionEnMinutos { get; set; }
       

    }
}
