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

        public async Task<AuthResult?> Register(UserCreateDto user)
        {
            var usernameExists = await _userRepository.GetUserByUsername(user.Username!);
            var emailExists = await _userRepository.GetUserByEmail(user.Email!);

            if (usernameExists == null && emailExists == null)
            {
                var applicationUser = new ApplicationUser
                {
                    UserName = user.Username,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };

                var createdUserId = await _userRepository.CreateUser(applicationUser, user.Password!);
                applicationUser.Id = createdUserId;

                var token = GenerateJwtToken(applicationUser);

                return new AuthResult()
                {
                    Result = true,
                    Token = token,
                };
            }

            return null;
        }

        public async Task<AuthResult?> Login(UserLoginDto user)
        {
            var applicationUser = await _userRepository.GetUserByUsername(user.Username!);
     
            if (applicationUser != null)
            {
                var valid = await _userRepository.CheckPasswordIsValid(applicationUser, user.Password!);

                if (valid)
                {
                    var token = GenerateJwtToken(applicationUser);

                    return new AuthResult()
                    {
                        Result = true,
                        Token = token,
                    };
                }
            }

            return null;
        }
        public Task<bool> DeleteUser(string username)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserDetails(string id, UserUpdateDetailsDto user)
        {
            var applicationUser = new ApplicationUser()
            {
                Id = id,
                FirstName = user.FirstName,
                LastName = user.FirstName,
            };

            return _userRepository.UpdateUserDetails(applicationUser);
        }

        public Task<bool> UpdateUserPassword(string id, UserUpdatePasswordDto user)
        {
            var applicationUser = new ApplicationUser()
            {
                Id = id,
            };

            return _userRepository.UpdateUserPassword(applicationUser, user.OldPassword!, user.NewPassword!);
        }

        // Utils
        private string GenerateJwtToken(ApplicationUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret!);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Aud, "ucms-app"),
                    new Claim(JwtRegisteredClaimNames.Iss, "ucms-api")
                }),

                Expires = DateTime.Now.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
