using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using User_Contact_Management_System.Controllers;
using User_Contact_Management_System.Dtos.Contacts;
using User_Contact_Management_System.Models;
using User_Contact_Management_System.Services.Contacts;
using User_Contact_Management_System.Utils;

namespace UCMS_test
{
    public class ContactControllerTests
    {
        private readonly Mock<UserUtils> _mockUserUtils;
        private readonly Mock<ILogger<IContactService>> _mockLogger;
        private readonly Mock<IContactService> _mockContactService;
        private readonly ContactsController _contactsController;
        private readonly ClaimsPrincipal user;

        public ContactControllerTests()
        {
            _mockUserUtils = new Mock<UserUtils>();
            _mockLogger = new Mock<ILogger<IContactService>>();
            _mockContactService = new Mock<IContactService>();
            _contactsController = new ContactsController(
                _mockLogger.Object, _mockContactService.Object, _mockUserUtils.Object);

            user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "TestUserId"),
            }));
            _contactsController.ControllerContext = new ControllerContext();
        }

        [Fact]
        public async Task CreateContact_ValidRequest_ReturnsCreatedAtAction()
        {
            // Arrange
            _contactsController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var userId = "TestUserId";
            var contactId = Guid.NewGuid();
            var contactCreateDto = new ContactCreateDto();
            var contact = new Contact { Id = contactId };

            var contactReturnDto = new ContactReturnDto { Id = contactId.ToString() };
            _mockContactService.Setup(x => x.CreateContact(userId, contactCreateDto)).ReturnsAsync(contactReturnDto);

            // Act
            var result = await _contactsController.CreateContact(contactCreateDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(ContactsController.GetContact), createdAtActionResult.ActionName);
            Assert.Equal(contactReturnDto, createdAtActionResult.Value);
        }

        [Fact]
        public async Task CreateContact_InvalidUser_ReturnsAuthorized()
        {
            // Arrange
            _contactsController.ControllerContext.HttpContext = new DefaultHttpContext { User = null };

            var contactCreateDto = new ContactCreateDto();
            

            // Act
            var result = await _contactsController.CreateContact(contactCreateDto);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetAllContacts_ValidRequest_ReturnsOk()
        {
            // Arrange
            _contactsController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            var userId = "TestUserId";
            var contactId1 = Guid.NewGuid();
            var contactId2 = Guid.NewGuid();
            var contactId3 = Guid.NewGuid();

            var contacts = new[]
            {
                new ContactReturnDto { Id = contactId1.ToString() },
                new ContactReturnDto { Id = contactId2.ToString() },
                new ContactReturnDto { Id = contactId3.ToString() }
            };

            _mockContactService.Setup(x => x.GetAllContacts(userId)).ReturnsAsync(contacts);

            // Act
            var result = await _contactsController.GetAllContacts();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(contacts, okObjectResult.Value);
        }

        [Fact]
        public async Task GetAllContacts_InvalidUser_ReturnsUnauthorized()
        {
            // Arrange
            _contactsController.ControllerContext.HttpContext = new DefaultHttpContext { User = null };

            // Act
            var result = await _contactsController.GetAllContacts();

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task GetContact_ValidRequest_ReturnsOk()
        {
            // Arrange
            _contactsController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            
            var userId = "TestUserId";
            var contactId = "contact1";
            var contact = new ContactReturnDto { Id = contactId };

            _mockContactService.Setup(x => x.GetContact(userId, contactId)).ReturnsAsync(contact);

            // Act
            var result = await _contactsController.GetContact(contactId);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(contact, okObjectResult.Value);
        }

        [Fact]
        public async Task GetContact_InvalidUser_ReturnsUnauthorized()
        {
            // Arrange
            _contactsController.ControllerContext.HttpContext = new DefaultHttpContext { User = null };

            // Act
            var result = await _contactsController.GetContact("id");

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task UpdateContact_ValidRequest_ReturnsOk()
        {
            // Arrange
            _contactsController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            
            var userId = "TestUserId";
            var contactId = "contact1";
            var contactUpdateDto = new ContactUpdateDto { };

            _mockContactService.Setup(x => x.UpdateContact(userId, contactId, contactUpdateDto)).ReturnsAsync(true);

            // Act
            var result = await _contactsController.UpdateContact(contactId, contactUpdateDto);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(contactUpdateDto, okObjectResult.Value);
        }

        [Fact]
        public async Task UpdateContact_InvalidUser_ReturnsUnauthorized()
        {
            // Arrange
            _contactsController.ControllerContext.HttpContext = new DefaultHttpContext { User = null };

            // Act
            var result = await _contactsController.UpdateContact("id", new ContactUpdateDto());

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task DeleteContact_ValidRequest_ReturnsOk()
        {
            // Arrange
            _contactsController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            
            var userId = "TestUserId";
            var contactId = "contact1";
            _mockContactService.Setup(x => x.DeleteContact(userId, contactId)).ReturnsAsync(true);

            // Act
            var result = await _contactsController.DeleteContact(contactId);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okObjectResult.Value);
        }

        [Fact]
        public async Task DeleteContact_InvalidUser_ReturnsUnauthorized()
        {
            // Arrange
            _contactsController.ControllerContext.HttpContext = new DefaultHttpContext { User = null };

            // Act
            var result = await _contactsController.DeleteContact("id");

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

    }
}
