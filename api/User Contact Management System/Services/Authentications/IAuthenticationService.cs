namespace User_Contact_Management_System.Services.Authentications
{
    public interface IAuthenticationService
    {
        Task<string?> GenerateJwtToken(string username);
    }
}
