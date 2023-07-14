using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace User_Contact_Management_System.Models
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public User? User { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? BillingAddress { get; set; }
    }
}
