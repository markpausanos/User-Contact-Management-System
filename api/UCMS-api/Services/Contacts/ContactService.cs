using AutoMapper;
using User_Contact_Management_System.Dtos.Contacts;
using User_Contact_Management_System.Models;
using User_Contact_Management_System.Repositories.Contacts;

namespace User_Contact_Management_System.Services.Contacts
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public ContactService(IContactRepository contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public async Task<ContactReturnDto?> CreateContact(string userId, ContactCreateDto contact)
        {
            var newContact = _mapper.Map<Contact>(contact);
            newContact.ApplicationUser = new ApplicationUser { Id = userId };

            var contactCreated = await _contactRepository.CreateContact(newContact);

            return _mapper.Map<ContactReturnDto>(contactCreated);
        }

        public async Task<IEnumerable<ContactReturnDto>> GetAllContacts(string userId)
        {
            var user = new ApplicationUser { Id = userId };
            var contacts = await _contactRepository.GetAllContacts(userId);
            return contacts.Select(contact =>
            {
                contact.ApplicationUser = user;
                return _mapper.Map<ContactReturnDto>(contact);
            }
            );
        }

        public async Task<ContactReturnDto?> GetContact(string userId, string id)
        {
            var user = new ApplicationUser { Id = userId };
            var contact = await _contactRepository.GetContact(userId, id);

            if (contact == null)
                return null;

            contact.ApplicationUser = user;

            return _mapper.Map<ContactReturnDto>(contact);
        }

        public async Task<bool> UpdateContact(string userId, string contactId, ContactUpdateDto contact)
        {
            var contactExists = await _contactRepository.GetContact(userId, contactId);

            if (contactExists == null)
                return false;

            contactExists.FirstName = contact.FirstName ?? contactExists.FirstName;
            contactExists.LastName = contact.LastName ?? contactExists.LastName;
            contactExists.ContactNumber = contact.ContactNumber ?? contactExists.ContactNumber;
            contactExists.EmailAddress = contact.EmailAddress ?? contactExists.EmailAddress;
            contactExists.BillingAddress = contact.BillingAddress ?? contactExists.BillingAddress;
            contactExists.DeliveryAddress = contact.DeliveryAddress ?? contactExists.DeliveryAddress;   

            return await _contactRepository.UpdateContact(contactExists);
        }

        public async Task<bool> DeleteContact(string userId, string id)
        {
            var contact = await _contactRepository.GetContact(userId, id);

            if (contact == null)
                return false;

            return await _contactRepository.DeleteContact(contact);
        }
    }
}
