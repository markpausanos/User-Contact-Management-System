using User_Contact_Management_System.Dtos.Contacts;

namespace User_Contact_Management_System.Services.Contacts
{
    public interface IContactService
    {
        /// <summary>
        /// Create a new Contact for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user for whom the contact will be created.</param>
        /// <param name="contact">Contact details to be created.</param>
        /// <returns>The created Contact or null if creation failed.</returns>
        Task<ContactReturnDto?> CreateContact(string userId, ContactCreateDto contact);

        /// <summary>
        /// Retrieve all contacts of a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose contacts to retrieve.</param>
        /// <returns>A collection of Contact entities.</returns>
        Task<IEnumerable<ContactReturnDto>> GetAllContacts(string userId);

        /// <summary>
        /// Retrieve a specific contact of a user.
        /// </summary>
        /// <param name="userId">The ID of the user whose contact to retrieve.</param>
        /// <param name="id">The ID of the contact to retrieve.</param>
        /// <returns>The Contact entity or null if not found.</returns>
        Task<ContactReturnDto?> GetContact(string userId, string id);

        /// <summary>
        /// Update a contact for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user for whom the contact will be updated.</param>
        /// <param name="contactId">The ID of the contact to be updated.</param>
        /// <param name="contact">The updated contact details.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
        Task<bool> UpdateContact(string userId, string contactId, ContactUpdateDto contact);

        /// <summary>
        /// Delete a contact for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user for whom the contact will be deleted.</param>
        /// <param name="id">The ID of the contact to be deleted.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        Task<bool> DeleteContact(string userId, string id);
    }
}
