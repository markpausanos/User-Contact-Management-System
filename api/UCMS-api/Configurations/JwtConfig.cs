namespace User_Contact_Management_System.Configurations
{
    public class JwtConfig
    {
        public string? Secret { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public TimeSpan ExpirationTimeFrame { get; set; }
    }
}
