using System.ComponentModel.DataAnnotations;

namespace User_Contact_Management_System.Dtos.Users
{
    public class UserUpdateDetailsDto
    {
        [MaxLength(50, ErrorMessage = "First name can have at most 50 characters")]
        public string? FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "Last name can have at most 50 characters")]
        public string? LastName { get; set; }
    }
}
