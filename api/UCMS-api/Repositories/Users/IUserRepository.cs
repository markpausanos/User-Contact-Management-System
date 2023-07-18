using Microsoft.AspNetCore.Identity;
using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Repositories.Users
{
    public interface IUserRepository
    {
        Task<string?> CreateUser(ApplicationUser user, string password);
        Task<ApplicationUser?> GetUserById(string id);
        Task<ApplicationUser?> GetUserByUsername(string username);
        Task<ApplicationUser?> GetUserByEmail(string email);
        Task<bool> CheckPasswordIsValid(ApplicationUser user, string password);
        Task<bool> UpdateUserDetails(ApplicationUser user);
        Task<bool> UpdateUserPassword(ApplicationUser user, string oldPassword, string newPassword);
    }
}
