using User_Contact_Management_System.Dtos.Contacts;

namespace User_Contact_Management_System.Services.Contacts
{
    public interface IContactService
    {
        Task<ContactReturnDto?> CreateContact(string userId, ContactCreateDto contact);
        Task<IEnumerable<ContactReturnDto>> GetAllContacts(string userId);
        Task<ContactReturnDto?> GetContact(string userId, string id);
        Task<bool> UpdateContact(string userId, string contactId, ContactUpdateDto contact);
        Task<bool> DeleteContact(string userId, string id);
    }
}
