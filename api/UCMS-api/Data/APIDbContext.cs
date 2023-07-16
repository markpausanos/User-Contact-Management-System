using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Data
{
    public class APIDbContext : IdentityDbContext<ApplicationUser>
    {
        public APIDbContext(DbContextOptions<APIDbContext> options) : base(options) { }
        public DbSet<Contact> Contacts { get; set; }
    }
}
