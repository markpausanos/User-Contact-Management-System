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
        public Task<ApplicationUser> GetUserByUsername(string username) =>
            _userManager.FindByNameAsync(username);

        public Task<ApplicationUser> GetUserByEmail(string email) =>
            _userManager.FindByEmailAsync(email);

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
        public Task<bool> DeleteUser(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> UpdateUserDetails(ApplicationUser user)
        {
            try
            {
                var applicationUser = await _userManager.FindByIdAsync(user.Id);

                applicationUser.FirstName = user.FirstName ?? applicationUser.FirstName;
                applicationUser.LastName = user.LastName ?? applicationUser.LastName;

                return await _userManager.UpdateAsync(applicationUser) != null;
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
                var applicationUser = await _userManager.FindByIdAsync(user.Id);

                if (!await _userManager.CheckPasswordAsync(applicationUser, oldPassword))
                {
                    return false;
                }

                applicationUser.PasswordHash = _userManager.PasswordHasher.HashPassword(applicationUser, newPassword);

                return await _userManager.UpdateAsync(applicationUser) != null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}