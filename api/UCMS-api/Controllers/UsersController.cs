using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Contact_Management_System.Configurations;
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
        private readonly JwtConfig _jwtConfig;

        public UsersController(
            ILogger<IUserService> logger, 
            IUserService userService, 
            UserUtils userUtils, 
            JwtConfig jwtConfig)
        {
            _logger = logger;
            _userService = userService;
            _userUtils = userUtils;
            _jwtConfig = jwtConfig;
        }

        /// <summary>
        /// Creates a new User
        /// </summary>
        /// <param name="user">User to be created</param>
        /// <returns>Returns an AuthResult</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/Users/Register
        ///     {
        ///         {
        ///             "firstName": "string",
        ///             "lastName": "string",
        ///             "email": "user@example.com",
        ///             "username": "string",
        ///             "password": "string",
        ///             "confirmPassword": "string"
        ///         }
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Successfully created User</response>
        /// <response code="400">User credentials invalid or already exist</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("Register")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserCreateDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return GetBadRequest("Invalid request.");

                var authResult = await _userService.Register(user);

                if (authResult == null)
                    return GetBadRequest("Username or Email already exists.");

                SetTokens(authResult.Token!, authResult.RefreshToken!);

                return Ok(authResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(_userService.Register)} threw an exception");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Generate a JWT Token
        /// </summary>
        /// <param name="user">User to be logged in</param>
        /// <returns>Returns an AuthResult</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/Users/Login
        ///     {
        ///         {
        ///             "username": "string",
        ///             "password": "string",
        ///         }
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Successfully generated AuthResult</response>
        /// <response code="400">User credentials invalid</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("Login")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return GetBadRequest("Invalid request.");

                var authResult = await _userService.Login(user);

                if (authResult == null)
                    return GetBadRequest("Invalid credentials.");

                SetTokens(authResult.Token!, authResult.RefreshToken!);

                return Ok(authResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(_userService.Login)} threw an exception");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Refreshes the JWT Token
        /// </summary>
        /// <param name="tokenRequest">Refresh token of the user</param>
        /// <returns>Returns an AuthResult</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/Users/Tokens/Refresh
        ///     {
        ///         "token": "string",
        ///         "refreshToken": "string"
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Successfully refreshed AuthResult</response>
        /// <response code="400">Invalid token</response>
        /// <response code="500">Internal server error</response>
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

                SetTokens(authResult.Token!, authResult.RefreshToken!);

                return Ok(authResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(_userService.VerifyToken)} threw an exception");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves the current logged in User
        /// </summary>
        /// <returns>Returns user details</returns>
        /// <response code="200">Successfully fetched user data</response>
        /// <response code="400">Invalid request</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var accessToken = Request.Cookies["AccessToken"];
                var userId = _userUtils.GetCurrentUser(HttpContext);

                if (userId == null)
                    return GetBadRequest("Invalid request.");

                var user = await _userService.GetUser(userId);

                return Ok(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(_userService.GetUser)} threw an exception");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates the details of a User
        /// </summary>
        /// <param name="user">Details of the user to be updated</param>
        /// <returns>Returns status of the update operation</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/Users/Details
        ///     {
        ///         {
        ///             "firstName": "string",
        ///             "lastName": "string",
        ///             "email": "user@example.com",
        ///         }
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Successfully updated User</response>
        /// <response code="400">Invalid user or parameters</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("Details")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Updates the password of a User
        /// </summary>
        /// <param name="user">Details of the user password to be updated</param>
        /// <returns>Returns status of the update operation</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/Users
        ///     {
        ///         {
        ///             "oldPassword": "string",
        ///             "newPassword": "string",
        ///             "confirmPassword": "string"
        ///         }
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Successfully updated User password</response>
        /// <response code="400">Invalid user or parameters</response>
        /// <response code="500">Internal server error</response>
        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]
        [Authorize]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Returns a BadRequest with an AuthResult containing an error message.
        /// </summary>
        /// <param name="errorMessage">The error message to return in the AuthResult</param>
        /// <returns>Returns a BadRequest with an AuthResult containing an error message</returns>

        private IActionResult GetBadRequest(string errorMessage)
        {
            return BadRequest(new AuthResult
            {
                Result = false,
                Token = null,
                Error = errorMessage
            });
        }

        /// <summary>
        /// Sets the Access and Refresh tokens as cookies on the client.
        /// </summary>
        /// <param name="token">The JWT token.</param>
        /// <param name="refreshToken">The refresh token.</param>
        private void SetTokens(string token, string refreshToken)
        {
            var RefreshTokenCookieOptions = new CookieOptions()
            {
                HttpOnly = false,
                Expires = DateTime.UtcNow.AddMonths(2),
                Path = "/",
                IsEssential = true,
                SameSite = SameSiteMode.None,
                Secure = true
            };

            var TokenCookieOptions = new CookieOptions()
            {
                HttpOnly = false,
                Expires = DateTime.UtcNow.Add(_jwtConfig.ExpirationTimeFrame),
                Path = "/",
                IsEssential = true,
                SameSite = SameSiteMode.None,
                Secure = true,
            };

            Response.Cookies.Append("AccessToken", token, TokenCookieOptions);
            Response.Cookies.Append("RefreshToken", refreshToken, RefreshTokenCookieOptions);
        }
    }
}
