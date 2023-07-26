using System.ComponentModel.DataAnnotations;

namespace User_Contact_Management_System.Dtos.Contacts
{
    public class ContactUpdateDto
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string? ContactNumber { get; set; }

        [EmailAddress]
        public string? EmailAddress { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? BillingAddress { get; set; }
    }
}
