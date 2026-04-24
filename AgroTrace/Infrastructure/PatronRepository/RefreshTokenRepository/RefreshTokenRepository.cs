using AgroTrace.Domain.Entities;
using AgroTrace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgroTrace.Infrastructure.PatronRepository.RefreshTokenRepository
{
    public class RefreshTokenRepository: IRefreshRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;

        }

        public async Task<RefreshToken?> GetByToken(string token)
        {
            return await _context.RefreshTokens
                .Include(r => r.Usuario)
                .ThenInclude(u => u.Rol)
                .FirstOrDefaultAsync(r => r.Token == token);
        }

        public async Task Add(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
        }


    }
}
