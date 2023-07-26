using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Repositories.Users
{
    public interface IUserRepository
    {
        /// <summary>
        /// Create a new ApplicationUser in the database.
        /// </summary>
        /// <param name="user">User details to be created.</param>
        /// <param name="password">Password for the user.</param>
        /// <returns>The created ApplicationUser or null if creation failed.</returns>
        Task<ApplicationUser?> CreateUser(ApplicationUser user, string password);

        /// <summary>
        /// Retrieve a specific ApplicationUser by their ID from the database.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The ApplicationUser entity or null if not found.</returns>
        Task<ApplicationUser?> GetUserById(string id);

        /// <summary>
        /// Retrieve a specific ApplicationUser by their username from the database.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>The ApplicationUser entity or null if not found.</returns>
        Task<ApplicationUser?> GetUserByUsername(string username);

        /// <summary>
        /// Retrieve a specific ApplicationUser by their email from the database.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>The ApplicationUser entity or null if not found.</returns>
        Task<ApplicationUser?> GetUserByEmail(string email);

        /// <summary>
        /// Checks if the provided password is valid for the given user.
        /// </summary>
        /// <param name="user">The ApplicationUser entity for which to validate the password.</param>
        /// <param name="password">The password to check.</param>
        /// <returns>A boolean indicating whether the password is valid.</returns>
        Task<bool> CheckPasswordIsValid(ApplicationUser user, string password);

        /// <summary>
        /// Updates a user's details in the database.
        /// </summary>
        /// <param name="user">The updated ApplicationUser details.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
        Task<bool> UpdateUserDetails(ApplicationUser user);

        /// <summary>
        /// Updates a user's password in the database.
        /// </summary>
        /// <param name="user">The ApplicationUser entity for which to update the password.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
        Task<bool> UpdateUserPassword(ApplicationUser user, string oldPassword, string newPassword);
    }
}
