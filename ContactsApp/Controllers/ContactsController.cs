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
        private readonly ILogger<ContactsController> _logger;

        public ContactsController(IContactService contactService, ILogger<ContactsController> logger)
        {
            _contactService = contactService ?? throw new ArgumentNullException(nameof(contactService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<List<Contact>>> GetAllContacts()
        {
            try
            {
                var contacts = await _contactService.GetAllContactsAsync();
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching all contacts.");
                return StatusCode(500, "An unexpected error occurred while fetching contacts.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(Guid id)
        {
            try
            {
                var contact = await _contactService.GetContactByIdAsync(id);
                if (contact == null)
                {
                    return NotFound("Contact not found.");
                }
                return Ok(contact);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching contact with ID: {Id}", id);
                return StatusCode(500, "An unexpected error occurred while fetching the contact.");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Contact>> CreateContact([FromBody] ContactDto contactDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdContact = await _contactService.CreateContactAsync(contactDto);
                return CreatedAtAction(nameof(GetContact), new { id = createdContact.Id }, createdContact);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error while creating contact with email: {Email}", contactDto?.Email);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating contact with email: {Email}", contactDto?.Email);
                return StatusCode(500, "An unexpected error occurred while creating the contact.");
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Contact>> UpdateContact(Guid id, [FromBody] ContactDto contactDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedContact = await _contactService.UpdateContactAsync(id, contactDto);
                if (updatedContact == null)
                {
                    return NotFound("Contact not found.");
                }
                return Ok(updatedContact);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error while updating contact ID: {Id}", id);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating contact ID: {Id}", id);
                return StatusCode(500, "An unexpected error occurred while updating the contact.");
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteContact(Guid id)
        {
            try
            {
                var deleted = await _contactService.DeleteContactAsync(id);
                if (!deleted)
                {
                    return NotFound("Contact not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while deleting contact ID: {Id}", id);
                return StatusCode(500, "An unexpected error occurred while deleting the contact.");
            }
        }
    }
}