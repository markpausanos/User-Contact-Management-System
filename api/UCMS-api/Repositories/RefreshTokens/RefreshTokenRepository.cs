using Microsoft.EntityFrameworkCore;
using User_Contact_Management_System.Data;
using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Repositories.RefreshTokens
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly APIDbContext _context;

        public RefreshTokenRepository(APIDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> CreateRefreshToken(RefreshToken refreshToken)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var applicationUser = await _context.Users.FindAsync(refreshToken.ApplicationUserId);

                    if (applicationUser == null)
                        return null;

                    refreshToken.ApplicationUserId = applicationUser.Id;
                    await _context.RefreshTokens.AddAsync(refreshToken);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return refreshToken;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<RefreshToken?> GetRefreshToken(string refreshToken)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var refreshTokenObject = await _context.RefreshTokens
                        .Where(refreshTokenExists =>
                        refreshTokenExists.Token == refreshToken &&
                        !refreshTokenExists.IsUsed && !refreshTokenExists.IsRevoked &&
                        refreshTokenExists.Expires > DateTime.Now
                        ).FirstOrDefaultAsync();

                    return refreshTokenObject;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<bool> SetTokenRevoked(RefreshToken refreshToken)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var refreshTokenObject = await _context.RefreshTokens
                        .FirstOrDefaultAsync(refreshTokenExists => refreshTokenExists.Token == refreshToken.Token);

                    if (refreshTokenObject == null)
                        return false;

                    refreshTokenObject.IsRevoked = true;
                    _context.Entry(refreshTokenObject).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<bool> SetTokenUsed(RefreshToken refreshToken)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var refreshTokenObject = await _context.RefreshTokens
                        .FirstOrDefaultAsync(refreshTokenExists => refreshTokenExists.Token == refreshToken.Token);

                    if (refreshTokenObject == null)
                        return false;

                    refreshTokenObject.IsRevoked = true;
                    _context.Entry(refreshTokenObject).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
