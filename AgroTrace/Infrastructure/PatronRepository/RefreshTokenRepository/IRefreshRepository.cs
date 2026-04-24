using AgroTrace.Domain.Entities;

namespace AgroTrace.Infrastructure.PatronRepository.RefreshTokenRepository
{
    public interface IRefreshRepository
    {
        Task<RefreshToken?> GetByToken(string token);
        Task Add(RefreshToken token);

    }
}
