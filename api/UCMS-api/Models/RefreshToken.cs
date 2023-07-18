using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User_Contact_Management_System.Models
{
    [Index(nameof(Token), IsUnique = true)]
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("ApplicationUserId")]
        public virtual string? ApplicationUserId { get; set; }
        public string? Token { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; }
        public bool IsUsed { get; set; } = false;
        public bool IsRevoked { get; set; } = false;
    }
}
