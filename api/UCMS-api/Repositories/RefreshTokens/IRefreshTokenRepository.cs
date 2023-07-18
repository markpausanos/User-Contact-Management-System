using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Repositories.RefreshTokens
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> CreateRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken?> GetRefreshToken(string refreshToken);
        Task<bool> SetTokenUsed(RefreshToken refreshToken);
        Task<bool> SetTokenRevoked(RefreshToken refreshToken);
    }
}
