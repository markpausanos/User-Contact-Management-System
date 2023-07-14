namespace User_Contact_Management_System.Models
{
    public class Contact
    {
        public Guid Id { get; set; }
        public User? User { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? BillingAddress { get; set; }
    }
}
