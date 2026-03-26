using System;

namespace AgroTrace.Domain.Entities
{
    public class Usuario: BaseEntity 
    {
        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Username { get; set; }

        public string? Email { get; set; }

        public string PasswordHash { get; set; }

        public int RolId { get; set; }
        public Rol Rol { get; set; }

        public ICollection<Finca> Fincas { get; set; } = new List<Finca>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
