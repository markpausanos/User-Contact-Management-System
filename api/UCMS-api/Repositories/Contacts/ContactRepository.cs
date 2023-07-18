using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using User_Contact_Management_System.Data;
using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Repositories.Contacts
{
    public class ContactRepository : IContactRepository
    {
        private readonly APIDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ContactRepository(APIDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Contact?> CreateContact(Contact contact)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var applicationUser = await _context.Users.FindAsync(contact.ApplicationUser!.Id);

                    contact.ApplicationUser = applicationUser;
                    await _context.Contacts.AddAsync(contact);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return contact;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<IEnumerable<Contact>> GetAllContacts(string userId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var contacts = await _context.Contacts
                        .Where(contact => contact.ApplicationUser!.Id == userId).ToListAsync();

                    return contacts;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<Contact?> GetContact(string userId, string id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var contact = await _context.Contacts
                        .FirstOrDefaultAsync(contact => contact.ApplicationUser!.Id == userId 
                        && contact.Id.ToString() == id);

                    return contact;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<bool> UpdateContact(Contact contact)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Entry(contact).CurrentValues.SetValues(contact);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task<bool> DeleteContact(Contact contact)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Entry(contact).State = EntityState.Deleted;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
