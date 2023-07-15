using System.ComponentModel.DataAnnotations;

namespace User_Contact_Management_System.Dtos.Users
{
    public class UserCreateDto
    {
        [Required]
        [MinLength(4, ErrorMessage = "First name too short")]
        [MaxLength(50, ErrorMessage = "First name can have at most 50 characters")]
        public string? FirstName { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Last name too short")]
        [MaxLength(50, ErrorMessage = "Last name can have at most 50 characters")]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email must be valid")]
        public string? Email { get; set; }

        [Required]
        [MinLength(4, ErrorMessage = "Username too short")]
        [MaxLength(15, ErrorMessage = "Username can have at most 15 characters")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password too short")]
        [MaxLength(50, ErrorMessage = "Password can have at most 50 characters")]
        public string? Password { get; set; }
    }
}
