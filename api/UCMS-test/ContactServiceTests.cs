using AutoMapper;
using Moq;
using User_Contact_Management_System.Dtos.Contacts;
using User_Contact_Management_System.Models;
using User_Contact_Management_System.Repositories.Contacts;
using User_Contact_Management_System.Services.Contacts;

namespace UCMS_test
{
    public class ContactServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IContactRepository> _mockContactRepository;
        private readonly ContactService _contactService;

        public ContactServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockContactRepository = new Mock<IContactRepository>();
            _contactService = new ContactService(_mockMapper.Object, _mockContactRepository.Object);
        }

        [Fact]
        public async Task CreateContact_WithValidData_ReturnsContact()
        {
            // Arrange

            var id = Guid.NewGuid();
            var userId = "TestUserId";

            var contactCreateDto = new ContactCreateDto
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                ContactNumber = "123123123",
                EmailAddress = "test@test.com",
                BillingAddress = "TestAddress",
                DeliveryAddress = "TestAddress"
            };

            var contact = new Contact
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                ContactNumber = "123123123",
                EmailAddress = "test@test.com",
                BillingAddress = "TestAddress",
                DeliveryAddress = "TestAddress"
            };

            var contactCreated = new Contact
            {
                Id = id,
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                ContactNumber = "123123123",
                EmailAddress = "test@test.com",
                BillingAddress = "TestAddress",
                DeliveryAddress = "TestAddress"
            };

            var contactReturnDto = new ContactReturnDto
            {
                Id = id.ToString(),
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                ContactNumber = "123123123",
                EmailAddress = "test@test.com",
                BillingAddress = "TestAddress",
                DeliveryAddress = "TestAddress"
            };


            _mockMapper
                .Setup(x => x.Map<Contact>(contactCreateDto))
                .Returns(contact);

            _mockContactRepository
                .Setup(x => x.CreateContact(contact))
                .ReturnsAsync(contactCreated);

            _mockMapper
                .Setup(x => x.Map<ContactReturnDto>(contactCreated))
                .Returns(contactReturnDto);

            // Act
            var result = await _contactService.CreateContact(userId, contactCreateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(contactReturnDto, result);

            _mockMapper.Verify(x => x.Map<Contact>(contactCreateDto), Times.Once);
            _mockContactRepository.Verify(x => x.CreateContact(contact), Times.Once);
            _mockMapper.Verify(x => x.Map<ContactReturnDto>(contactCreated), Times.Once);
        }

        [Fact]
        public async Task CreateContact_WithInvalidData_ReturnsContact()
        {
            // Arrange

            var id = Guid.NewGuid();
            var userId = "TestUserId";

            var contactCreateDto = new ContactCreateDto
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                ContactNumber = "123123123",
                EmailAddress = "test@test.com",
                BillingAddress = "TestAddress",
                DeliveryAddress = "TestAddress"
            };

            var contact = new Contact
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                ContactNumber = "123123123",
                EmailAddress = "test@test.com",
                BillingAddress = "TestAddress",
                DeliveryAddress = "TestAddress"
            };


            _mockMapper
                .Setup(x => x.Map<Contact>(contactCreateDto))
                .Returns(contact);

            _mockContactRepository
                .Setup(x => x.CreateContact(contact))
                .ReturnsAsync(() => null);

            _mockMapper
                .Setup(x => x.Map<ContactReturnDto>(null))
                .Returns((ContactReturnDto)null);

            // Act
            var result = await _contactService.CreateContact(userId, contactCreateDto);

            // Assert
            Assert.Null(result);
            _mockMapper.Verify(x => x.Map<Contact>(contactCreateDto), Times.Once);
            _mockContactRepository.Verify(x => x.CreateContact(It.IsAny<Contact>()), Times.Once);
            _mockMapper.Verify(x => x.Map<ContactReturnDto>(It.IsAny<Contact>()), Times.Once);
        }

        [Fact]
        public async Task GetAllContacts_ValidUserId_ReturnsListOfContactReturnDto()
        {
            // Arrange
            var userId = "TestUserId";
            var contactId1 = Guid.NewGuid();
            var contactId2 = Guid.NewGuid();
            var contactId3 = Guid.NewGuid();

            var contacts = new List<Contact>
            {
                new Contact { Id = contactId1 },
                new Contact { Id = contactId2 },
                new Contact { Id = contactId3 }
            };

            var contactReturnDtos = contacts
                .Select(c => new ContactReturnDto { Id = c.Id.ToString(), ApplicationUserId = userId });


            _mockContactRepository
                .Setup(x => x.GetAllContacts(userId))
                .ReturnsAsync(contacts);

            _mockMapper
                .Setup(x => x.Map<ContactReturnDto>(It.IsAny<Contact>()))
                .Returns((Contact c) => contactReturnDtos.FirstOrDefault(dto => dto.Id == c.Id.ToString()));

            // Act
            var result = await _contactService.GetAllContacts(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equivalent(contactReturnDtos, result, strict: false);

            _mockContactRepository.Verify(x => x.GetAllContacts(userId), Times.Once);
            _mockMapper.Verify(x => x.Map<ContactReturnDto>(It.IsAny<Contact>()), Times.Exactly(contacts.Count));
        }

        [Fact]
        public async Task GetContact_ExistingContact_ReturnsContactReturnDto()
        {
            // Arrange
            var userId = "TestUserId";
            var contactId = Guid.NewGuid();

            var contact = new Contact { Id = contactId };
            var contactReturnDto = new ContactReturnDto { Id = contactId.ToString() };

            _mockContactRepository
                .Setup(x => x.GetContact(userId, contactId.ToString()))
                .ReturnsAsync(contact);

            _mockMapper
                .Setup(x => x.Map<ContactReturnDto>(contact))
                .Returns(contactReturnDto);


            // Act
            var result = await _contactService.GetContact(userId, contactId.ToString());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(contactReturnDto, result);

            _mockContactRepository.Verify(x => x.GetContact(userId, contactId.ToString()), Times.Once);
            _mockMapper.Verify(x => x.Map<ContactReturnDto>(contact), Times.Once);
        }

        [Fact]
        public async Task GetContact_NonexistentContact_ReturnsNull()
        {
            // Arrange
            var userId = "TestUserId";
            var contactId = "contact1";
            
            _mockContactRepository
                .Setup(x => x.GetContact(userId, contactId))
                .ReturnsAsync((Contact)null);

            // Act
            var result = await _contactService.GetContact(userId, contactId);

            // Assert
            Assert.Null(result);

            _mockContactRepository.Verify(x => x.GetContact(userId, contactId), Times.Once);
            _mockMapper.Verify(x => x.Map<ContactReturnDto>(It.IsAny<Contact>()), Times.Never);
        }

        [Fact]
        public async Task UpdateContact_ExistingContact_ReturnsTrue()
        {
            // Arrange
            var userId = "TestUserId";
            var contactId = Guid.NewGuid();

            var contactUpdateDto = new ContactUpdateDto 
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                ContactNumber = "123123123",
                EmailAddress = "test@test.com",
                BillingAddress = "TestAddress",
                DeliveryAddress = "TestAddress"
            };

            var existingContact = new Contact { Id = contactId };


            _mockContactRepository
                .Setup(x => x.GetContact(userId, contactId.ToString()))
                .ReturnsAsync(existingContact);

            _mockContactRepository
                .Setup(x => x.UpdateContact(existingContact))
                .ReturnsAsync(true);

            var contactService = new ContactService(Mock.Of<IMapper>(), _mockContactRepository.Object);

            // Act
            var result = await contactService.UpdateContact(userId, contactId.ToString(), contactUpdateDto);

            // Assert
            Assert.True(result);

            _mockContactRepository.Verify(x => x.GetContact(userId, contactId.ToString()), Times.Once);
            _mockContactRepository.Verify(x => x.UpdateContact(existingContact), Times.Once);
        }

        [Fact]
        public async Task UpdateContact_NonexistentContact_ReturnsFalse()
        {
            // Arrange
            var userId = "TestUserId";
            var contactId = "contact1";
            var contactUpdateDto = new ContactUpdateDto
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                ContactNumber = "123123123",
                EmailAddress = "test@test.com",
                BillingAddress = "TestAddress",
                DeliveryAddress = "TestAddress"
            };

            var _mockContactRepository = new Mock<IContactRepository>();

            _mockContactRepository
                .Setup(x => x.GetContact(userId, contactId))
                .ReturnsAsync((Contact)null);

            var contactService = new ContactService(Mock.Of<IMapper>(), _mockContactRepository.Object);

            // Act
            var result = await contactService.UpdateContact(userId, contactId, contactUpdateDto);

            // Assert
            Assert.False(result);

            _mockContactRepository.Verify(x => x.GetContact(userId, contactId), Times.Once);
            _mockContactRepository.Verify(x => x.UpdateContact(It.IsAny<Contact>()), Times.Never);
        }

        [Fact]
        public async Task DeleteContact_ExistingContact_ReturnsTrue()
        {
            // Arrange
            var userId = "TestUserId";
            var contactId = Guid.NewGuid();
            var existingContact = new Contact { Id = contactId };


            _mockContactRepository
                .Setup(x => x.GetContact(userId, contactId.ToString()))
                .ReturnsAsync(existingContact);

            _mockContactRepository
                .Setup(x => x.DeleteContact(existingContact))
                .ReturnsAsync(true);

            var contactService = new ContactService(Mock.Of<IMapper>(), _mockContactRepository.Object);

            // Act
            var result = await contactService.DeleteContact(userId, contactId.ToString());

            // Assert
            Assert.True(result);

            _mockContactRepository.Verify(x => x.GetContact(userId, contactId.ToString()), Times.Once);
            _mockContactRepository.Verify(x => x.DeleteContact(existingContact), Times.Once);
        }

        [Fact]
        public async Task DeleteContact_NonexistentContact_ReturnsFalse()
        {
            // Arrange
            var userId = "TestUserId";
            var contactId = "contact1";

            var _mockContactRepository = new Mock<IContactRepository>();

            _mockContactRepository
                .Setup(x => x.GetContact(userId, contactId))
                .ReturnsAsync((Contact)null);

            var contactService = new ContactService(Mock.Of<IMapper>(), _mockContactRepository.Object);

            // Act
            var result = await contactService.DeleteContact(userId, contactId);

            // Assert
            Assert.False(result);

            _mockContactRepository.Verify(x => x.GetContact(userId, contactId), Times.Once);
            _mockContactRepository.Verify(x => x.DeleteContact(It.IsAny<Contact>()), Times.Never);
        }
    }
}
