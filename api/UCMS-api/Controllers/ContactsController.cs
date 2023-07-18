using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User_Contact_Management_System.Dtos.Contacts;
using User_Contact_Management_System.Services.Contacts;
using User_Contact_Management_System.Utils;

namespace User_Contact_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly ILogger<IContactService> _logger;
        private readonly IContactService _contactService;
        private readonly UserUtils _userUtils;

        public ContactsController(ILogger<IContactService> logger, IContactService contactService, UserUtils userUtils)
        {
            _logger = logger;
            _contactService = contactService;
            _userUtils = userUtils;
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact([FromBody] ContactCreateDto contact)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid request.");

                var userId = _userUtils.GetCurrentUser(HttpContext);

                if (userId == null)
                    return Unauthorized();

                var createdContact = await _contactService.CreateContact(userId, contact);

                return createdContact != null ? CreatedAtAction(nameof(GetContact), new { id = createdContact }, createdContact)
                    : BadRequest("Invalid parameters");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(_contactService.CreateContact)} threw an exception");
                return StatusCode(500, "Internal server error");
            }
        }

            
        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid request.");

                var userId = _userUtils.GetCurrentUser(HttpContext);

                if (userId == null)
                    return Unauthorized();


                var contacts = await _contactService.GetAllContacts(userId);

                return contacts != null ? Ok(contacts) : BadRequest("Invalid parameters");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(_contactService.GetContact)} threw an exception");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContact([FromRoute] string id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid request.");

                var userId = _userUtils.GetCurrentUser(HttpContext);

                if (userId == null)
                    return Unauthorized();


                var contact = await _contactService.GetContact(userId, id);

                return contact != null ? Ok(contact) : BadRequest("Invalid parameters");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(_contactService.GetContact)} threw an exception");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact([FromRoute] string id, [FromBody] ContactUpdateDto contact)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid request.");

                var userId = _userUtils.GetCurrentUser(HttpContext);

                if (userId == null)
                    return Unauthorized();


                var contactUpdated = await _contactService.UpdateContact(userId, id, contact);

                return contact != null ? Ok(contact) : BadRequest("Invalid parameters");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(_contactService.GetContact)} threw an exception");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact([FromRoute] string id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid request.");

                var userId = _userUtils.GetCurrentUser(HttpContext);

                if (userId == null)
                    return Unauthorized();


                var isDeleted = await _contactService.DeleteContact(userId, id);

                return isDeleted ? Ok(isDeleted) : BadRequest("Invalid parameters");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(_contactService.GetContact)} threw an exception");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
