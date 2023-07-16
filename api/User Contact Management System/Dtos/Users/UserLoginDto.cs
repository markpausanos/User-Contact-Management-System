using System.ComponentModel.DataAnnotations;

namespace User_Contact_Management_System.Dtos.Users
{
    public class UserLoginDto
    {
        [Required]
        [MinLength(4, ErrorMessage = "Invalid username")]
        [MaxLength(15, ErrorMessage = "Invalid username")]
        public string? Username { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Invalid password")]
        public string? Password { get; set; }
    }
}
