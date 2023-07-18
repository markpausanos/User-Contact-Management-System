namespace User_Contact_Management_System.Models
{
    public class AuthResult
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public bool Result { get; set; }
        public string? Error { get; set; }
    }
}
