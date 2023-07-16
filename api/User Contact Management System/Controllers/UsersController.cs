using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User_Contact_Management_System.Dtos.Users;
using User_Contact_Management_System.Models;
using User_Contact_Management_System.Services.Users;

namespace User_Contact_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<IUserService> _logger;
        private readonly IUserService _userService;

        public UsersController(ILogger<IUserService> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        private IActionResult GetBadRequest(string errorMessage)
        {
            return BadRequest(new AuthResult()
            {
                Result = false,
                Token = null,
                Error = errorMessage
            });
        }

        [HttpPost("Register")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserCreateDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return GetBadRequest("Invalid request.");

                var authResult = await _userService.Register(user);

                return authResult != null ? Ok(authResult) : GetBadRequest("Username or Email already exists.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(_userService.Register)} threw an exception");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("Login")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return GetBadRequest("Invalid request.");

                var authResult = await _userService.Login(user);

                return authResult != null ? Ok(authResult) : GetBadRequest("Invalid credentials.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(_userService.Login)} threw an exception");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("UpdateDetails")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UserUpdateDetailsDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return GetBadRequest("Invalid request.");

                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                    return GetBadRequest("Invalid user.");

                var result = await _userService.UpdateUserDetails(userId, user);

                return result ? Ok(result) : GetBadRequest("Invalid parameters.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(_userService.UpdateUserDetails)} threw an exception");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("UpdatePassword")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(UserUpdatePasswordDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return GetBadRequest("Invalid request.");

                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                    return GetBadRequest("Invalid user.");

                var result = await _userService.UpdateUserPassword(userId, user);

                return result ? Ok(result) : GetBadRequest("Invalid parameters.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(_userService.UpdateUserPassword)} threw an exception");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
