using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Repositories.Contacts
{
    public interface IContactRepository
    {
        Task<Contact?> CreateContact(Contact contact);
        Task<IEnumerable<Contact>> GetAllContacts(string userId);
        Task<Contact?> GetContact(string userId, string id);
        Task<bool> UpdateContact(Contact contact);
        Task<bool> DeleteContact(Contact contact);
    }
}
