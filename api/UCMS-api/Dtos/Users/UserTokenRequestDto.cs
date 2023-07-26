using System.ComponentModel.DataAnnotations;

namespace User_Contact_Management_System.Dtos.Users
{
    public class UserTokenRequestDto
    {
        [Required]
        public string? RefreshToken { get; set; }
    }
}
