using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using User_Contact_Management_System.Configurations;
using User_Contact_Management_System.Dtos.Users;
using User_Contact_Management_System.Models;
using User_Contact_Management_System.Repositories.RefreshTokens;
using User_Contact_Management_System.Repositories.Users;

namespace User_Contact_Management_System.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public UserService(
            IMapper mapper,
            IUserRepository userRepository, 
            IRefreshTokenRepository refreshTokenRepository,
            JwtConfig jwtConfig,
            TokenValidationParameters tokenValidationParameters
            )
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtConfig = jwtConfig;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<AuthResult?> Register(UserCreateDto user)
        {
            var usernameExists = await _userRepository.GetUserByUsername(user.Username!);
            var emailExists = await _userRepository.GetUserByEmail(user.Email!);

            if (usernameExists != null || emailExists != null)
                return null;

           
            var applicationUser = _mapper.Map<ApplicationUser>(user);   

            var createdUser = await _userRepository.CreateUser(applicationUser, user.Password!);

            if (createdUser == null)
                return null;

            applicationUser = createdUser;

            return await GetAuthResult(applicationUser);
        }

        public async Task<AuthResult?> Login(UserLoginDto user)
        {
            var applicationUser = await _userRepository.GetUserByUsername(user.Username!);

            if (applicationUser == null)
                return null;

            var valid = await _userRepository.CheckPasswordIsValid(applicationUser, user.Password!);

            if (!valid)
                return null;

            return await GetAuthResult(applicationUser);
        }

        public async Task<bool> Logout(UserTokenRequestDto tokenRequest)
        {
            return await _refreshTokenRepository.SetTokenRevoked(new RefreshToken
            {
                Token = tokenRequest.RefreshToken
            });
        }

        public async Task<AuthResult?> VerifyToken(UserTokenRequestDto tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                //var tokenInValidation = 
                //     jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);

                //if (!(validatedToken is JwtSecurityToken jwtSecurityToken))
                //    return null;

                //var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                //    StringComparison.InvariantCultureIgnoreCase);

                //if (!result)
                //    return null;

                //var utcExpiryDate = long.Parse(tokenInValidation.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)!.Value);
                //var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                //var expiryDate = dateTime.AddSeconds(utcExpiryDate).ToUniversalTime();

                //if (expiryDate > DateTime.Now)
                //    return null;

                var storedToken = await _refreshTokenRepository.GetRefreshToken(tokenRequest.RefreshToken!);

                if (storedToken == null)
                    return null;

                if (storedToken.Expires < DateTime.UtcNow)
                    return null;

                await _refreshTokenRepository.SetTokenUsed(storedToken);

                var user = await _userRepository.GetUserById(storedToken.ApplicationUserId!);

                if (user == null)
                    return null;

                return await GetAuthResult(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<UserReturnDto?> GetUser(string id)
        {
            var applicationUser = await _userRepository.GetUserById(id);

            if (applicationUser == null)
                return null;

            return _mapper.Map<UserReturnDto>(applicationUser);
        }
        public async Task<bool> UpdateUserDetails(string id, UserUpdateDetailsDto user)
        {
            var applicationUser = await _userRepository.GetUserById(id);

            if (applicationUser == null)
                return false;
            

            applicationUser.FirstName = user.FirstName ?? applicationUser.FirstName;
            applicationUser.LastName = user.LastName ?? applicationUser.LastName;

            return await _userRepository.UpdateUserDetails(applicationUser);
        }

        public async Task<bool> UpdateUserPassword(string id, UserUpdatePasswordDto user)
        {
            var applicationUser = await _userRepository.GetUserById(id);

            if (applicationUser == null)
                return false;
            

            return await _userRepository.UpdateUserPassword(applicationUser, user.OldPassword!, user.NewPassword!);
        }

        // Utils
        private async Task<AuthResult?> GetAuthResult(ApplicationUser applicationUser)
        {
            var token = GenerateJwtToken(applicationUser);

            var refreshTokenString = GenerateRefreshToken(applicationUser.Id);
            var refreshTokenObject = await _refreshTokenRepository.CreateRefreshToken(refreshTokenString);

            return new AuthResult
            {
                Result = true,
                Token = token,
                RefreshToken = refreshTokenObject!.Token
            };
        }
        private string GenerateJwtToken(ApplicationUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret!);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Aud, _jwtConfig.Audience!),
                    new Claim(JwtRegisteredClaimNames.Iss, _jwtConfig.Issuer!)
                }),

                Expires = DateTime.UtcNow.Add(_jwtConfig.ExpirationTimeFrame),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            
            return jwtTokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(string userId)
        {
            var refreshToken = new RefreshToken
            {
                ApplicationUserId = userId,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddMonths(2)
            };

            return refreshToken;
        }
    }
}
