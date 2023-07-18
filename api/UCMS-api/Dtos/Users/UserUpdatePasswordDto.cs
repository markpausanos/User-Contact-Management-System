using FoolProof.Core;
using System.ComponentModel.DataAnnotations;

namespace User_Contact_Management_System.Dtos.Users
{
    public class UserUpdatePasswordDto
    {
        [Required]
        [MinLength(6, ErrorMessage = "Password too short")]
        [MaxLength(50, ErrorMessage = "Password can have at most 50 characters")]
        public string? OldPassword { get; set; }

        [Required]
        [NotEqualTo(nameof(OldPassword))]
        [MinLength(6, ErrorMessage = "Password too short")]
        [MaxLength(50, ErrorMessage = "Password can have at most 50 characters")]
        public string? NewPassword { get; set; }

        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set; }
    }
}
