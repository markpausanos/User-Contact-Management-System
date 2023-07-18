using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Contact_Management_System.Dtos.Users;
using User_Contact_Management_System.Models;
using User_Contact_Management_System.Services.Users;
using User_Contact_Management_System.Utils;

namespace User_Contact_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<IUserService> _logger;
        private readonly IUserService _userService;
        private readonly UserUtils _userUtils;

        public UsersController(ILogger<IUserService> logger, IUserService userService, UserUtils userUtils)
        {
            _logger = logger;
            _userService = userService;
            _userUtils = userUtils;
        }

        [HttpPost("Register")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserCreateDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return GetBadRequest("Invalid request.");

                var authResult = await _userService.Register(user);

                if (authResult == null)
                    return GetBadRequest("Username or Email already exists.");

                SetRefreshToken(authResult.RefreshToken!);

                return Ok(authResult);
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
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return GetBadRequest("Invalid request.");

                var authResult = await _userService.Login(user);

                if (authResult == null)
                    return GetBadRequest("Invalid credentials.");

                SetRefreshToken(authResult.RefreshToken!);

                return Ok(authResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(_userService.Login)} threw an exception");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("Tokens/Refresh")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> RefreshToken([FromBody] UserTokenRequestDto tokenRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return GetBadRequest("Invalid request.");

                var authResult = await _userService.VerifyToken(tokenRequest);

                if (authResult == null)
                    return GetBadRequest("Invalid token.");

                SetRefreshToken(authResult.RefreshToken!);

                return Ok(authResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(_userService.VerifyToken)} threw an exception");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("Details")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDetailsDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return GetBadRequest("Invalid request.");

                var userId = _userUtils.GetCurrentUser(HttpContext);

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

        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody] UserUpdatePasswordDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return GetBadRequest("Invalid request.");

                var userId = _userUtils.GetCurrentUser(HttpContext);

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
        private IActionResult GetBadRequest(string errorMessage)
        {
            return BadRequest(new AuthResult()
            {
                Result = false,
                Token = null,
                Error = errorMessage
            });
        }

        private void SetRefreshToken(string refreshToken)
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddMonths(2)
            };

            Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
        }
    }
}
