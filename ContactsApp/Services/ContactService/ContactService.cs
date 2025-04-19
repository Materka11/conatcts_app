using ContactsApp.Data;
using ContactsApp.Entities;
using ContactsApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ContactsApp.Services.ContactService
{
    public class ContactService : IContactService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public ContactService(AppDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<List<Contact>> GetAllContactsAsync()
        {
            return await _context.Contacts.ToListAsync();
        }

        public async Task<Contact?> GetContactByIdAsync(int id)
        {
            return await _context.Contacts.FindAsync(id);
        }

        public async Task<Contact> CreateContactAsync(ContactDto contactDto)
        {
            //walidacja
            ValidateContactDto(contactDto);

            //stworzenie nowego kontaktu
            var contact = new Contact
            {
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                Email = contactDto.Email,
                Category = contactDto.Category,
                Subcategory = contactDto.Subcategory,
                Phone = contactDto.Phone,
                DateOfBirth = contactDto.DateOfBirth
            };

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return contact;
        }

        public async Task<Contact?> UpdateContactAsync(int id, ContactDto contactDto)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact is null)
            {
                return null;
            }

            //walidacja inputa
            ValidateContactDto(contactDto, isUpdate: true);

            contact.FirstName = contactDto.FirstName;
            contact.LastName = contactDto.LastName;
            contact.Email = contactDto.Email;
            contact.Category = contactDto.Category;
            contact.Subcategory = contactDto.Subcategory;
            contact.Phone = contactDto.Phone;
            contact.DateOfBirth = contactDto.DateOfBirth;

            await _context.SaveChangesAsync();

            return contact;
        }

        public async Task<bool> DeleteContactAsync(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact is null)
            {
                return false;
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return true;
        }

        private void ValidateContactDto(ContactDto contactDto, bool isUpdate = false)
        {
            if (contactDto is null)
            {
                throw new ArgumentNullException(nameof(contactDto));
            }

            if (string.IsNullOrWhiteSpace(contactDto.FirstName) || string.IsNullOrWhiteSpace(contactDto.LastName))
            {
                throw new ArgumentException("First name and last name are required.");
            }

            if (string.IsNullOrWhiteSpace(contactDto.Email) || !Regex.IsMatch(contactDto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new ArgumentException("Invalid email format.");
            }

            if (!isUpdate && _context.Contacts.Any(c => c.Email == contactDto.Email))
            {
                throw new ArgumentException("Email already exists.");
            }

            if (!new[] { "Służbowy", "Prywatny", "Inny" }.Contains(contactDto.Category))
            {
                throw new ArgumentException("Invalid category. Must be Służbowy, Prywatny, or Inny.");
            }

            if (contactDto.Category == "Służbowy" && !new[] { "Szef", "Klient", "Pracownik" }.Contains(contactDto.Subcategory))
            {
                throw new ArgumentException("Invalid subcategory for Business. Must be Szef, Klient, or Pracownik.");
            }

            if (contactDto.Category == "Inny" && string.IsNullOrWhiteSpace(contactDto.Subcategory))
            {
                throw new ArgumentException("Subcategory is required for Other category.");
            }
        }
    }
}