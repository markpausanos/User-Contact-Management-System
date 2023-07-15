using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Repositories.Users
{
    public interface IUserRepository
    {
        Task<int> CreateUser(User user);
        Task<User?> GetUserByUsername(string username);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(int id);
    }
}
