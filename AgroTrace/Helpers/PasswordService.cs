using AgroTrace.Domain.Entities;
using Microsoft.AspNetCore.Identity;
namespace AgroTrace.Helpers
{
    public class PasswordService
    {
        private readonly IPasswordHasher<Usuario> _hasher;

        public PasswordService(IPasswordHasher<Usuario> hasher)
        {
            _hasher = hasher;
        }

        public string HashPassword(Usuario usuario, string password)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password inválida");

            return _hasher.HashPassword(usuario, password);
        }

        public bool VerifyPassword(Usuario usuario, string password, string hash)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException("Hash inválido");

            var result = _hasher.VerifyHashedPassword(usuario, hash, password);

            return result == PasswordVerificationResult.Success ||
                   result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
