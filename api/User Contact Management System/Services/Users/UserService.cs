using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User_Contact_Management_System.Configurations;
using User_Contact_Management_System.Dtos.Users;
using User_Contact_Management_System.Models;
using User_Contact_Management_System.Repositories.Users;

namespace User_Contact_Management_System.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtConfig _jwtConfig;

        public UserService(IUserRepository userRepository, JwtConfig jwtConfig)
        {
            _userRepository = userRepository;
            _jwtConfig = jwtConfig; 
        }

        public async Task<AuthResult?> CreateUser(UserCreateDto user)
        {
            var userExists = await _userRepository.GetUserByUsername(user.Username!);

            if (userExists == null)
            {
                var identityUser = new ApplicationUser
                {
                    UserName = user.Username,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };

                var createdUserId = await _userRepository.CreateUser(identityUser, user.Password!);
                identityUser.Id = createdUserId;

                var token = GenerateJwtToken(identityUser);

                return new AuthResult()
                {
                    Result = true,
                    Token = token,
                };
            }

            return null;
        }

        public Task<bool> DeleteUser(string username)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser?> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser?> GetUserByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserByUsername(string username)
        {
            throw new NotImplementedException();
        }

        // Utils
        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret!);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim("Username", user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                }),

                Expires = DateTime.Now.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
