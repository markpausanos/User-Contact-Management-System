using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Repositories.RefreshTokens
{
    public interface IRefreshTokenRepository
    {
        /// <summary>
        /// Create a new refresh token in the database.
        /// </summary>
        /// <param name="refreshToken">Refresh token details to be created.</param>
        /// <returns>The created RefreshToken or null if creation failed.</returns>
        Task<RefreshToken?> CreateRefreshToken(RefreshToken refreshToken);

        /// <summary>
        /// Retrieve a specific refresh token from the database.
        /// </summary>
        /// <param name="refreshToken">The token string of the refresh token to retrieve.</param>
        /// <returns>The RefreshToken entity or null if not found.</returns>
        Task<RefreshToken?> GetRefreshToken(string refreshToken);

        /// <summary>
        /// Set a specific refresh token as used in the database.
        /// </summary>
        /// <param name="refreshToken">The RefreshToken entity to be set as used.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
        Task<bool> SetTokenUsed(RefreshToken refreshToken);

        /// <summary>
        /// Set a specific refresh token as revoked in the database.
        /// </summary>
        /// <param name="refreshToken">The RefreshToken entity to be set as revoked.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
        Task<bool> SetTokenRevoked(RefreshToken refreshToken);
    }
}
