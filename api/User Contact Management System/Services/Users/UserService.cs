using User_Contact_Management_System.Dtos.Users;
using User_Contact_Management_System.Models;
using User_Contact_Management_System.Repositories.Users;

namespace User_Contact_Management_System.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> CreateUser(UserCreateDto user)
        {
            var newuser = new User { 
                FirstName = user.FirstName,
                Email = user.Email, 
                LastName = user.Email,  
                Username = user.Username,
            };

            await _userRepository.CreateUser(newuser);

            return newuser;
        }

        public Task<bool> DeleteUser(string username)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserByUsername(string username)
        {
            throw new NotImplementedException();
        }
    }
}
