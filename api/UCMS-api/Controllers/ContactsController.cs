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

        /// <summary>
        /// Creates a new Contact for a specific User
        /// </summary>
        /// <param name="contact">Contact to be created</param>
        /// <returns>Returns the created Contact</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/Contacts
        ///     {
        ///         "firstName": "string",
        ///         "lastName": "string",
        ///         "contatNumber": "string",
        ///         "emailAddress": "contact@example.com",
        ///         "deliveryAddress": "string",
        ///         "billingAddress": "string"
        ///     }
        ///     
        /// </remarks>
        /// <response code="201">Successfully created Contact</response>
        /// <response code="400">Invalid request or parameters</response>
        /// <response code="401">Unauthorized request</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ContactReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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


        // <summary>
        /// Retrieves all Contacts for a specific User
        /// </summary>
        /// <returns>Returns all Contacts for a User</returns>
        /// <response code="200">Successfully fetched all Contacts</response>
        /// <response code="400">Invalid request</response>
        /// <response code="401">Unauthorized request</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ContactReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Retrieves a Contact by its ID for a specific User
        /// </summary>
        /// <param name="id">ID of the Contact to be retrieved</param>
        /// <returns>Returns the Contact</returns>
        /// <response code="200">Successfully fetched Contact</response>
        /// <response code="400">Invalid request or parameters</response>
        /// <response code="401">Unauthorized request</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContactReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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


        /// <summary>
        /// Updates a Contact by its ID for a specific User
        /// </summary>
        /// <param name="id">ID of the Contact to be updated</param>
        /// <param name="contact">New details of the Contact</param>
        /// <returns>Returns the updated Contact</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/Contacts/{id}
        ///     {
        ///         "firstName": "string",
        ///         "lastName": "string",
        ///         "contatNumber": "string",
        ///         "emailAddress": "contact@example.com",
        ///         "deliveryAddress": "string",
        ///         "billingAddress": "string"
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Successfully updated Contact</response>
        /// <response code="400">Invalid request or parameters</response>
        /// <response code="401">Unauthorized request</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Deletes a Contact by its ID for a specific User
        /// </summary>
        /// <param name="id">ID of the Contact to be deleted</param>
        /// <returns>Returns status of the delete operation</returns>
        /// <response code="200">Successfully deleted Contact</response>
        /// <response code="400">Invalid request or parameters</response>
        /// <response code="401">Unauthorized request</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
