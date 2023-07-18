using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using User_Contact_Management_System.Controllers;
using User_Contact_Management_System.Services.Users;
using User_Contact_Management_System.Dtos.Users;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using User_Contact_Management_System.Models;
using User_Contact_Management_System.Utils;

namespace UCMS_test
{
    public class UsersControllerTest
    {
        private readonly Mock<UserUtils> _mockUserUtils;
        private readonly Mock<ILogger<IUserService>> _mockLogger;
        private readonly Mock<IUserService> _mockUserService;
        private readonly UsersController _usersController;

        public UsersControllerTest()
        {
            _mockUserUtils = new Mock<UserUtils>();
            _mockLogger = new Mock<ILogger<IUserService>>();
            _mockUserService = new Mock<IUserService>();
            _usersController = new UsersController(_mockLogger.Object, _mockUserService.Object, _mockUserUtils.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "TestUserId"),
            }));

            _usersController.ControllerContext = new ControllerContext();
            _usersController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
        }

        [Fact]
        public async Task Register_WithValidData_ReturnsOk()
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

            var authResult = new AuthResult
            {
                Result = true,
                Token = "TestToken",
                RefreshToken = "TestRefreshToken"
            };

            _mockUserService.Setup(x => x.Register(userCreateDto))
                .ReturnsAsync(authResult);

            // Act
            var result = await _usersController.Register(userCreateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(authResult, okResult.Value);
        }

        [Fact]
        public async Task Register_WithExistingUsernameOrEmail_ReturnsBadRequest()
        {
            // Arrange
            var userCreateDto = new UserCreateDto
            {
                Username = "existinguser",
                Email = "existinguser@example.com",
                Password = "Test1234",
                FirstName = "Test",
                LastName = "User"
            };

            _mockUserService.Setup(x => x.Register(userCreateDto))
                .ReturnsAsync(() => null);

            // Act
            var result = await _usersController.Register(userCreateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedAuthResult = Assert.IsType<AuthResult>(badRequestResult.Value);
            Assert.False(returnedAuthResult.Result);
            Assert.Null(returnedAuthResult.Token);
            Assert.Equal("Username or Email already exists.", returnedAuthResult.Error);
        }

        [Fact]
        public async Task Login_WithValidData_ReturnsOk()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Username = "testuser",
                Password = "Test1234"
            };

            var authResult = new AuthResult
            {
                Result = true,
                Token = "TestToken",
                RefreshToken = "TestRefreshToken"
            };

            _mockUserService.Setup(x => x.Login(userLoginDto))
                .ReturnsAsync(authResult);

            // Act
            var result = await _usersController.Login(userLoginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(authResult, okResult.Value);
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ReturnsBadRequest()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Username = "invaliduser",
                Password = "invalidpassword"
            };

            _mockUserService.Setup(x => x.Login(userLoginDto))
                .ReturnsAsync(() => null);

            // Act
            var result = await _usersController.Login(userLoginDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedAuthResult = Assert.IsType<AuthResult>(badRequestResult.Value);
            Assert.False(returnedAuthResult.Result);
            Assert.Null(returnedAuthResult.Token);
            Assert.Equal("Invalid credentials.", returnedAuthResult.Error);
        }

        [Fact]
        public async Task RefreshToken_WithValidRequest_ReturnsOk()
        {
            // Arrange
            var tokenRequest = new UserTokenRequestDto
            {
                Token = "TestToken",
                RefreshToken = "TestRefreshToken",
            };

            var authResult = new AuthResult
            {
                Result = true,
                Token = "TestNewToken",
                RefreshToken = "TestNewRefreshToken"
            };

            _mockUserService.Setup(x => x.VerifyToken(tokenRequest))
                .ReturnsAsync(authResult);

            // Act
            var result = await _usersController.RefreshToken(tokenRequest);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(authResult, okResult.Value);
        }


        [Fact]
        public async Task RefreshToken_WithInvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var tokenRequest = new UserTokenRequestDto
            {
                Token = "TestToken",
                RefreshToken = "TestRefreshToken",
            };

            _mockUserService.Setup(x => x.VerifyToken(tokenRequest))
                .ReturnsAsync(() => null);

            // Act
            var result = await _usersController.RefreshToken(tokenRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedAuthResult = Assert.IsType<AuthResult>(badRequestResult.Value);
            Assert.False(returnedAuthResult.Result);
            Assert.Null(returnedAuthResult.Token);
            Assert.Equal("Invalid token.", returnedAuthResult.Error);
        }

        [Fact]
        public async Task UpdateUser_WithValidData_ReturnsOk()
        {
            // Arrange
            var userUpdateDetailsDto = new UserUpdateDetailsDto
            {
                FirstName = "NewFirstName",
                LastName = "NewLastName"
            };

            _mockUserService.Setup(x => x.UpdateUserDetails(It.IsAny<string>(), userUpdateDetailsDto))
                .ReturnsAsync(true);

            // Act
            var result = await _usersController.UpdateUser(userUpdateDetailsDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task UpdateUser_WithInvalidParameters_ReturnsBadRequest()
        {
            // Arrange
            var userUpdateDetailsDto = new UserUpdateDetailsDto
            {
                FirstName = "NewFirstName",
                LastName = "NewLastName"
            };

            _mockUserService.Setup(x => x.UpdateUserDetails(It.IsAny<string>(), userUpdateDetailsDto))
                .ReturnsAsync(false);

            // Act
            var result = await _usersController.UpdateUser(userUpdateDetailsDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedAuthResult = Assert.IsType<AuthResult>(badRequestResult.Value);
            Assert.False(returnedAuthResult.Result);
            Assert.Null(returnedAuthResult.Token);
            Assert.Equal("Invalid parameters.", returnedAuthResult.Error);
        }

        [Fact]
        public async Task UpdatePassword_WithValidData_ReturnsOk()
        {
            // Arrange
            var userUpdatePasswordDto = new UserUpdatePasswordDto
            {
                OldPassword = "OldPassword",
                NewPassword = "NewPassword"
            };

            _mockUserService.Setup(x => x.UpdateUserPassword(It.IsAny<string>(), userUpdatePasswordDto))
                .ReturnsAsync(true);

            // Act
            var result = await _usersController.UpdatePassword(userUpdatePasswordDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task UpdatePassword_WithInvalidParameters_ReturnsBadRequest()
        {
            // Arrange
            var userUpdatePasswordDto = new UserUpdatePasswordDto
            {
                OldPassword = "InvalidOldPassword",
                NewPassword = "NewPassword"
            };

            _mockUserService.Setup(x => x.UpdateUserPassword(It.IsAny<string>(), userUpdatePasswordDto))
                .ReturnsAsync(false);

            // Act
            var result = await _usersController.UpdatePassword(userUpdatePasswordDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnedAuthResult = Assert.IsType<AuthResult>(badRequestResult.Value);
            Assert.False(returnedAuthResult.Result);
            Assert.Null(returnedAuthResult.Token);
            Assert.Equal("Invalid parameters.", returnedAuthResult.Error);
        }

    }
}
