using Moq;
using User_Contact_Management_System.Services.Users;
using User_Contact_Management_System.Dtos.Users;
using User_Contact_Management_System.Repositories.Users;
using User_Contact_Management_System.Models;
using User_Contact_Management_System.Configurations;
using User_Contact_Management_System.Repositories.RefreshTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;

namespace UCMS_test
{
    public class UserServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRefreshTokenRepository> _mockRefreshTokenRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;
        private readonly JwtConfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public UserServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _jwtConfig = new JwtConfig
            {
                Secret = "This is a very secret key",
                Audience = "Audience",
                Issuer = "Issuer",
                ExpirationTimeFrame = new TimeSpan(0, 1, 0)
            };
            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = false,
                ValidIssuer = _jwtConfig.Issuer,
                ValidAudience = _jwtConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret!))
            };
            _userService = new UserService(_mockMapper.Object, _mockUserRepository.Object, _mockRefreshTokenRepository.Object, _jwtConfig, _tokenValidationParameters);
        }

        [Fact]
        public async Task Register_WithValidData_ReturnsAuthResult()
        {
            // Arrange
            var userCreateDto = new UserCreateDto
            {
                Username = "testuser",
                Email = "testuser@example.com",
                Password = "Test1234",
                FirstName = "Test",
                LastName = "User"
            };

            var applicationUser = new ApplicationUser
            {
                Id = "NewId",
                Email = "Newemail@email.com"
            };

            var refreshToken = new RefreshToken
            {
                Token = "RefreshToken"
            };

            _mockUserRepository.Setup(x => x.GetUserByUsername(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _mockUserRepository.Setup(x => x.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _mockUserRepository.Setup(x => x.CreateUser(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(() => applicationUser);

            _mockRefreshTokenRepository.Setup(x => x.CreateRefreshToken(It.IsAny<RefreshToken>()))
                .ReturnsAsync(() => refreshToken);

            // Act
            var result = await _userService.Register(userCreateDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Result);
            Assert.NotNull(result.Token);
            Assert.NotNull(result.RefreshToken);
        }

        [Fact]
        public async Task Register_WithExistingUsername_ReturnsNull()
        {
            // Arrange
            var userCreateDto = new UserCreateDto
            {
                Username = "testuser",
                Email = "testuser@example.com",
                Password = "Test1234",
                FirstName = "Test",
                LastName = "User"
            };

            var existingUser = new ApplicationUser
            {
                UserName = "testuser"
            };

            _mockUserRepository.Setup(x => x.GetUserByUsername(It.IsAny<string>()))
                .ReturnsAsync(() => existingUser);

            // Act
            var result = await _userService.Register(userCreateDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Register_WithExistingEmail_ReturnsNull()
        {
            // Arrange
            var userCreateDto = new UserCreateDto
            {
                Username = "testuser",
                Email = "testuser@example.com",
                Password = "Test1234",
                FirstName = "Test",
                LastName = "User"
            };

            var existingUser = new ApplicationUser
            {
                Email = "testuser@example.com"
            };

            _mockUserRepository.Setup(x => x.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync(() => existingUser);

            // Act
            var result = await _userService.Register(userCreateDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Login_WithValidData_ReturnsAuthResult()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Username = "testuser",
                Password = "Test1234"
            };

            var applicationUser = new ApplicationUser
            {
                Id = "TestUserId",
                Email = "testuser@example.com",
                UserName = userLoginDto.Username
            };

            var refreshToken = new RefreshToken
            {
                Token = "RefreshToken"
            };

            _mockUserRepository.Setup(x => x.GetUserByUsername(It.IsAny<string>()))
                .ReturnsAsync(() => applicationUser);

            _mockUserRepository.Setup(x => x.CheckPasswordIsValid(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(() => true);

            _mockRefreshTokenRepository.Setup(x => x.CreateRefreshToken(It.IsAny<RefreshToken>()))
                .ReturnsAsync(() => refreshToken);

            // Act
            var result = await _userService.Login(userLoginDto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Result);
            Assert.NotNull(result.Token);
            Assert.NotNull(result.RefreshToken);
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ReturnsNull()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Username = "testuser",
                Password = "WrongPassword"
            };

            var applicationUser = new ApplicationUser
            {
                UserName = userLoginDto.Username
            };

            _mockUserRepository.Setup(x => x.GetUserByUsername(It.IsAny<string>()))
                .ReturnsAsync(() => applicationUser);

            _mockUserRepository.Setup(x => x.CheckPasswordIsValid(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(() => false);

            // Act
            var result = await _userService.Login(userLoginDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Login_WithNonexistentUsername_ReturnsNull()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Username = "nonexistentuser",
                Password = "Test1234"
            };

            _mockUserRepository.Setup(x => x.GetUserByUsername(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _userService.Login(userLoginDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateUserDetails_WithValidData_ReturnsTrue()
        {
            // Arrange
            var userUpdateDetailsDto = new UserUpdateDetailsDto
            {
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName"
            };

            var applicationUser = new ApplicationUser
            {
                Id = "TestUserId"
            };

            _mockUserRepository.Setup(x => x.GetUserById(It.IsAny<string>()))
                .ReturnsAsync(() => applicationUser);

            _mockUserRepository.Setup(x => x.UpdateUserDetails(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(() => true);

            // Act
            var result = await _userService.UpdateUserDetails("SomeUserID", userUpdateDetailsDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateUserPassword_WithValidData_ReturnsTrue()
        {
            // Arrange
            var userUpdatePasswordDto = new UserUpdatePasswordDto
            {
                OldPassword = "OldPassword",
                NewPassword = "NewPassword"
            };

            var applicationUser = new ApplicationUser
            {
                Id = "TestUserId"
            };

            _mockUserRepository.Setup(x => x.GetUserById(It.IsAny<string>()))
                .ReturnsAsync(() => applicationUser);

            _mockUserRepository.Setup(x => x.UpdateUserPassword(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => true);

            // Act
            var result = await _userService.UpdateUserPassword("SomeUserID", userUpdatePasswordDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateUserPassword_WithWrongOldPassword_ReturnsFalse()
        {
            // Arrange
            var userUpdatePasswordDto = new UserUpdatePasswordDto
            {
                OldPassword = "WrongOldPassword",
                NewPassword = "NewPassword"
            };

            _mockUserRepository.Setup(x => x.UpdateUserPassword(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => false);

            // Act
            var result = await _userService.UpdateUserPassword("SomeUserID", userUpdatePasswordDto);

            // Assert
            Assert.False(result);
        }
    }
}
