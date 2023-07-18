using FoolProof.Core;
using System.ComponentModel.DataAnnotations;

namespace User_Contact_Management_System.Dtos.Contacts
{
    public class ContactCreateDto
    {

        [RequiredIfEmpty(nameof(LastName))]
        public string? FirstName { get; set; }

        [RequiredIfEmpty(nameof(FirstName))]
        public string? LastName { get; set; }

        [Phone]
        public string? ContactNumber { get; set; }

        [EmailAddress]
        public string? EmailAddress { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? BillingAddress { get; set; }
    }
}
