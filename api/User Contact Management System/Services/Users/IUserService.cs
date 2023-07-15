using User_Contact_Management_System.Dtos.Users;
using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Services.Users
{
    public interface IUserService
    {
        Task<AuthResult?> CreateUser(UserCreateDto user);
        Task<ApplicationUser?> GetUserByUsername(string username);
        Task<ApplicationUser?> GetUserByEmail(string email);
        Task<bool> UpdateUserByUsername(string username);
        Task<bool> DeleteUser(string username);
    }
}
