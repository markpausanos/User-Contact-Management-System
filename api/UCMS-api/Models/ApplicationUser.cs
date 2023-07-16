using Microsoft.AspNetCore.Identity;

namespace User_Contact_Management_System.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Contact>? Contacts { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
