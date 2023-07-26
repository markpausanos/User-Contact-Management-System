using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Repositories.Contacts
{
    public interface IContactRepository
    {
        /// <summary>
        /// Create a new contact in the database.
        /// </summary>
        /// <param name="contact">Contact details to be created.</param>
        /// <returns>The created Contact or null if creation failed.</returns>
        Task<Contact?> CreateContact(Contact contact);

        /// <summary>
        /// Retrieve all contacts of a specific user from the database.
        /// </summary>
        /// <param name="userId">The ID of the user whose contacts to retrieve.</param>
        /// <returns>A collection of Contact entities.</returns>
        Task<IEnumerable<Contact>> GetAllContacts(string userId);

        /// <summary>
        /// Retrieve a specific contact of a user from the database.
        /// </summary>
        /// <param name="userId">The ID of the user whose contact to retrieve.</param>
        /// <param name="id">The ID of the contact to retrieve.</param>
        /// <returns>The Contact entity or null if not found.</returns>
        Task<Contact?> GetContact(string userId, string id);

        /// <summary>
        /// Update a contact in the database.
        /// </summary>
        /// <param name="contact">The updated contact details.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
        Task<bool> UpdateContact(Contact contact);

        /// <summary>
        /// Delete a contact from the database.
        /// </summary>
        /// <param name="contact">The contact to be deleted.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        Task<bool> DeleteContact(Contact contact);
    }
}
