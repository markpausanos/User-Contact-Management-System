using Microsoft.AspNetCore.Identity;
using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Repositories.Users
{
    public interface IUserRepository
    {
        Task<string?> CreateUser(ApplicationUser user, string password);
        Task<ApplicationUser?> GetUserByUsername(string username);
        Task<bool> UpdateUser(IdentityUser user);
        Task<bool> DeleteUser(int id);
    }
}
