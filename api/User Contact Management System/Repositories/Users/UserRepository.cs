using Microsoft.EntityFrameworkCore;
using User_Contact_Management_System.Data;
using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly APIDbContext _context;

        public UserRepository(APIDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateUser(User user)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                    return user.Id;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task<User?> GetUserByUsername(string username)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);        
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
        public Task<bool> DeleteUser(int id)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}