using System.ComponentModel.DataAnnotations;
using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Dtos.Contacts
{
    public class ContactReturnDto
    {
        public string? Id { get; set; }
        public string? ApplicationUserId { get; set; }
        public string? Image { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ContactNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? BillingAddress { get; set; }
    }
}
