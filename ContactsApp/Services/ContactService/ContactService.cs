using ContactsApp.Data;
using ContactsApp.Entities;
using ContactsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Services.ContactService
{
    public class ContactService(AppDbContext context, IConfiguration configuration) : IContactService
    {
        public async Task<List<Contact>> GetAllContactsAsync()
        {
            var contacts = await context.Contacts.ToListAsync();
            return contacts;
        }
        public async Task<Contact?> GetContactByIdAsync(int id)
        {
            var contact = await context.Contacts.FindAsync(id);
            if (contact is null)
            {
                return null;
            }

            return contact;
        }
        public Task<Contact> CreateContactAsync(ContactDto contactDto)
        {
            throw new NotImplementedException();
        }
        public Task<Contact?> UpdateContactAsync(int id, ContactDto contactDto)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteContactAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
    {
    }
}
