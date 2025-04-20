using ContactsApp.Entities;
using ContactsApp.Models;

namespace ContactsApp.Services.ContactService
{
    public interface IContactService
    {
        Task<List<Contact>> GetAllContactsAsync();
        Task<Contact?> GetContactByIdAsync(Guid id);
        Task<Contact> CreateContactAsync(ContactDto contactDto);
        Task<Contact?> UpdateContactAsync(Guid id, ContactDto contactDto);
        Task<bool> DeleteContactAsync(Guid id);
    }
}
