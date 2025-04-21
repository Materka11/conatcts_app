using ContactsApp.Entities;
using ContactsApp.Models;
using ContactsApp.Services.ContactService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService ?? throw new ArgumentNullException(nameof(contactService));
        }

        [HttpGet]
        public async Task<ActionResult<List<Contact>>> GetAllContacts()
        {
            var contacts = await _contactService.GetAllContactsAsync();
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(Guid id)
        {
            var contact = await _contactService.GetContactByIdAsync(id);

            if (contact is null)
            {
                return NotFound("Contact not found.");
            }

            return Ok(contact);
        }

        //tworzy nowy kontakt (tylko dla uwierzytelnionych)
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Contact>> CreateContact([FromBody] ContactDto contactDto)
        {
            try
            {
                var createdContact = await _contactService.CreateContactAsync(contactDto);
                return CreatedAtAction(nameof(GetContact), new { id = createdContact.Id }, createdContact);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //aktualizuje istniejący kontakt (tylko dla uwierzytelnionych)
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Contact>> UpdateContact(Guid id, [FromBody] ContactDto contactDto)
        {
            try
            {
                var updatedContact = await _contactService.UpdateContactAsync(id, contactDto);

                if (updatedContact is null)
                {
                    return NotFound("Contact not found.");
                }
                return Ok(updatedContact);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //usuwa kontakt (tylko dla uwierzytelnionych)
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteContact(Guid id)
        {
            var deleted = await _contactService.DeleteContactAsync(id);

            if (!deleted)
            {
                return NotFound("Contact not found.");
            }
            return NoContent();
        }
    }
}