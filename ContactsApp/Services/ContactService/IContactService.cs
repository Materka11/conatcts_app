using ContactsApp.Entities;
using ContactsApp.Models;

namespace ContactsApp.Services.ContactService
{
    public interface IContactService
    {
        Task<List<Contact>> GetAllContactsAsync();
        Task<Contact?> GetContactByIdAsync(int id);
        Task<Contact> CreateContactAsync(ContactDto contactDto);
        Task<Contact?> UpdateContactAsync(int id, ContactDto contactDto);
        Task<bool> DeleteContactAsync(int id);
    }
}
