using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ApplicationUser?> GetUserByUsername(string username)
        {
            try
            {
                if (username != null)
                {
                    return await _userManager.FindByNameAsync(username);
                }
  
                return null;
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
        public Task<bool> UpdateUser(IdentityUser user)
        {
            throw new NotImplementedException();
        }
    }
}