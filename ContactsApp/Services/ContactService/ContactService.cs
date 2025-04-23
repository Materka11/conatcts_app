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
            return await _context.Contacts
                 .Include(c => c.Category)
                 .Include(c => c.Subcategory)
                 .ToListAsync();
        }

        public async Task<Contact?> GetContactByIdAsync(Guid id)
        {
            return await _context.Contacts
                .Include(c => c.Category)
                .Include(c => c.Subcategory)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Contact> CreateContactAsync(ContactDto contactDto)
        {
            //walidacja inputa i wziecie id podkategorii
            var subcategoryId = await ValidateContactDtoAsync(contactDto);

            //stworzenie nowego kontaktu
            var contact = new Contact
            {
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                Email = contactDto.Email,
                CategoryId = contactDto.CategoryId,
                SubcategoryId = subcategoryId,
                Phone = contactDto.Phone,
                DateOfBirth = contactDto.DateOfBirth
            };

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return contact;
        }

        public async Task<Contact?> UpdateContactAsync(Guid id, ContactDto contactDto)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact is null)
            {
                return null;
            }

            //walidacja inputa i wziecie id podkategorii
            var subcategoryId = await ValidateContactDtoAsync(contactDto, isUpdate: true);

            contact.FirstName = contactDto.FirstName;
            contact.LastName = contactDto.LastName;
            contact.Email = contactDto.Email;
            contact.CategoryId = contactDto.CategoryId;
            contact.SubcategoryId = subcategoryId;
            contact.Phone = contactDto.Phone;
            contact.DateOfBirth = contactDto.DateOfBirth;

            await _context.SaveChangesAsync();

            return contact;
        }

        public async Task<bool> DeleteContactAsync(Guid id)
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

        private async Task<Guid?> ValidateContactDtoAsync(ContactDto contactDto, bool isUpdate = false)
        {
            if (contactDto is null)
            {
                throw new ArgumentNullException(nameof(contactDto));
            }

            //weryfikuje, czy imię i nazwisko nie są puste
            if (string.IsNullOrWhiteSpace(contactDto.FirstName) || string.IsNullOrWhiteSpace(contactDto.LastName))
            {
                throw new ArgumentException("First name and last name are required.");
            }

            //sprawdza poprawność formatu adresu email
            if (string.IsNullOrWhiteSpace(contactDto.Email) || !Regex.IsMatch(contactDto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new ArgumentException("Invalid email format.");
            }

            //przy tworzeniu nowego kontaktu sprawdza, czy email już nie istnieje w bazie
            if (!isUpdate && await _context.Contacts.AnyAsync(c => c.Email == contactDto.Email))
            {
                throw new ArgumentException("Email already exists.");
            }

            var category = await _context.Categories.FindAsync(contactDto.CategoryId);
            if (category is null)
            {
                throw new ArgumentException("Invalid category ID.");
            }

            Guid? subcategoryId = null;

            if (category.Name == "sluzbowy")
            {
                //sprawdza, czy podkategoria jest podana (wymagana dla "Służbowy")
                if (!contactDto.SubcategoryId.HasValue)
                {
                    throw new ArgumentException("Subcategory is required for 'Służbowy' category.");
                }

                //pobiera podkategorię na podstawie SubcategoryId i CategoryId
                var subcategory = await _context.Subcategories
                    .Where(s => s.Id == contactDto.SubcategoryId && s.CategoryId == contactDto.CategoryId)
                    .FirstOrDefaultAsync();

                //weryfikuje, czy podkategoria istnieje i należy do dozwolonych wartości dla "Służbowy"
                if (subcategory is null || !new[] { "Szef", "Klient", "Pracownik" }.Contains(subcategory.Name))
                {
                    throw new ArgumentException("Invalid subcategory for 'Służbowy' category. Must be Szef, Klient, or Pracownik.");
                }

                //przypisuje ID podkategorii
                subcategoryId = contactDto.SubcategoryId;
            }
            else if (category.Name == "inny")
            {
                //sprawdza, czy podano podkategorię (CustomSubcategory lub SubcategoryId)
                if (string.IsNullOrWhiteSpace(contactDto.CustomSubcategory) && !contactDto.SubcategoryId.HasValue)
                {
                    throw new ArgumentException("Subcategory is required for 'Inny' category.");
                }

                //jeśli podano niestandardową podkategorię
                if (!string.IsNullOrWhiteSpace(contactDto.CustomSubcategory))
                {
                    //sprawdza, czy podkategoria już istnieje w bazie
                    var existingSubcategory = await _context.Subcategories
                        .Where(s => s.Name == contactDto.CustomSubcategory && s.CategoryId == contactDto.CategoryId)
                        .FirstOrDefaultAsync();

                    //jeśli podkategoria nie istnieje, tworzy nową
                    if (existingSubcategory is null)
                    {
                        var newSubcategory = new Subcategory
                        {
                            Name = contactDto.CustomSubcategory,
                            CategoryId = contactDto.CategoryId
                        };
                        _context.Subcategories.Add(newSubcategory);
                        //zapisuje nową podkategorię w bazie
                        await _context.SaveChangesAsync();
                        subcategoryId = newSubcategory.Id;
                    }
                    //jeśli istnieje, używa jej ID
                    else
                    {
                        subcategoryId = existingSubcategory.Id;
                    }
                }
                //jeśli podano SubcategoryId zamiast niestandardowej podkategorii
                else
                {
                    //pobiera podkategorię na podstawie SubcategoryId
                    var subcategory = await _context.Subcategories
                        .Where(s => s.Id == contactDto.SubcategoryId && s.CategoryId == contactDto.CategoryId)
                        .FirstOrDefaultAsync();
                    if (subcategory is null)
                    {
                        throw new ArgumentException("Invalid subcategory ID for 'Inny' category.");
                    }

                    //przypisuje ID podkategorii
                    subcategoryId = contactDto.SubcategoryId;
                }
            }
            else if (contactDto.SubcategoryId.HasValue)
            {
                var subcategory = await _context.Subcategories
                    .Where(s => s.Id == contactDto.SubcategoryId && s.CategoryId == contactDto.CategoryId)
                    .FirstOrDefaultAsync();
                if (subcategory is null)
                {
                    throw new ArgumentException("Invalid subcategory ID.");
                }
                subcategoryId = contactDto.SubcategoryId;
            }

            //zwraca ID podkategorii (lub null, jeśli nie podano)
            return subcategoryId;
        }
    }
}