using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("SignUp", Name = "CreateUser")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> CreateUser(UserCreateDto user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var authResult = await _userService.CreateUser(user);

                    if (authResult == null)
                    {
                        return BadRequest(new AuthResult()
                        {
                            Result = false,
                            Token = null,
                            Error = "Username or Email already exists."
                        });
                    }

                    return Ok(authResult);
                }
           
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Token = null,
                    Error = "Invalid request."
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "An error occurred while creating the User.");
            }
        }
    }
}
