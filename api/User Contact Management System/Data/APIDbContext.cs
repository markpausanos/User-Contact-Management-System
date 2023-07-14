using Microsoft.EntityFrameworkCore;
using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Data
{
    public class APIDbContext : DbContext
    {
        private readonly string? _connectionString;

        public APIDbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
