using Microsoft.AspNetCore.Identity;
using User_Contact_Management_System.Data;
using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly APIDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserRepository(APIDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<string?> CreateUser(ApplicationUser user, string password)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var isCreated = await _userManager.CreateAsync(user, password);

                    if (isCreated.Succeeded)
                    {
                        return await _userManager.GetUserIdAsync(user);
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }

        }

        public async Task<ApplicationUser?> GetUserById(string id) =>
            await _userManager.FindByIdAsync(id);
        public async Task<ApplicationUser?> GetUserByUsername(string username) =>
            await _userManager.FindByNameAsync(username);

        public async Task<ApplicationUser?> GetUserByEmail(string email) =>
            await _userManager.FindByEmailAsync(email);

        public async Task<bool> CheckPasswordIsValid(ApplicationUser user, string password)
        {
            try
            {
                return await _userManager.CheckPasswordAsync(user, password);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> UpdateUserDetails(ApplicationUser user)
        {
            try
            {
                return await _userManager.UpdateAsync(user) != null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateUserPassword(ApplicationUser user, string oldPassword, string newPassword)
        {
            try
            {
                if (!await _userManager.CheckPasswordAsync(user, oldPassword))
                {
                    return false;
                }

                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, newPassword);

                return await _userManager.UpdateAsync(user) != null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}