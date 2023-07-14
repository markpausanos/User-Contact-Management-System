using Microsoft.EntityFrameworkCore;
using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Data
{
    public class APIDbContext : DbContext
    {
        public APIDbContext(DbContextOptions<APIDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
}
