using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace User_Contact_Management_System.Models
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? BillingAddress { get; set; }
    }
}
