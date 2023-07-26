using User_Contact_Management_System.Dtos.Users;
using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Services.Users
{
    public interface IUserService
    {
        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="user">User details to be registered.</param>
        /// <returns>An AuthResult containing authentication details or null if registration failed.</returns>
        Task<AuthResult?> Register(UserCreateDto user);

        /// <summary>
        /// Authenticate a user and generate tokens.
        /// </summary>
        /// <param name="user">User details for login.</param>
        /// <returns>An AuthResult containing authentication details or null if login failed.</returns>
        Task<AuthResult?> Login(UserLoginDto user);

        /// <summary>
        /// Log a user out and revoke the token.
        /// </summary>
        /// <param name="tokenRequest">Details of the token to be revoked.</param>
        /// <returns>A boolean indicating whether the logout was successful.</returns>
        Task<bool> Logout(UserTokenRequestDto tokenRequest);

        /// <summary>
        /// Verify a token's validity and issue new tokens if valid.
        /// </summary>
        /// <param name="tokenRequest">Details of the token to be verified.</param>
        /// <returns>An AuthResult containing new authentication details or null if token verification failed.</returns>
        Task<AuthResult?> VerifyToken(UserTokenRequestDto tokenRequest);

        /// <summary>
        /// Retrieve a specific user.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The User entity or null if not found.</returns>
        Task<UserReturnDto?> GetUser(string id);

        /// <summary>
        /// Update a user's details.
        /// </summary>
        /// <param name="id">The ID of the user to be updated.</param>
        /// <param name="user">The updated user details.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
        Task<bool> UpdateUserDetails(string id, UserUpdateDetailsDto user);

        /// <summary>
        /// Update a user's password.
        /// </summary>
        /// <param name="id">The ID of the user for which the password will be updated.</param>
        /// <param name="user">The details containing the old and new password.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
        Task<bool> UpdateUserPassword(string id, UserUpdatePasswordDto user);
    }
}
