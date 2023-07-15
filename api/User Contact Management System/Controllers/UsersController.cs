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
                var newUserToken = await _userService.CreateUser(user);

                if (newUserToken == null)
                {
                    return BadRequest("User cannot be created.");
                }

                return Ok(newUserToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "An error occurred while creating the User.");
            }
        }
    }
}
