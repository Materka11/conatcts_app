﻿using ContactsApp.Models;
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
        public async Task<ActionResult<List<ContactDto>>> GetAllContacts()
        {
            try
            {
                var contacts = await _contactService.GetAllContactsAsync();
                var contactDtos = contacts.Select(c => new ContactDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    CategoryId = c.CategoryId,
                    Category = c.Category != null ? new CategoryDto
                    {
                        Id = c.Category.Id,
                        Name = c.Category.Name
                    } : null,
                    SubcategoryId = c.SubcategoryId,
                    Subcategory = c.Subcategory != null ? new SubcategoryDto
                    {
                        Id = c.Subcategory.Id,
                        Name = c.Subcategory.Name,
                        CategoryId = c.Subcategory.CategoryId
                    } : null,
                    Phone = c.Phone,
                    DateOfBirth = c.DateOfBirth
                }).ToList();
                return Ok(contactDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching all contacts.");
                return StatusCode(500, "An unexpected error occurred while fetching contacts.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContactDto>> GetContact(Guid id)
        {
            try
            {
                var contact = await _contactService.GetContactByIdAsync(id);
                if (contact == null)
                {
                    return NotFound("Contact not found.");
                }
                var contactDto = new ContactDto
                {
                    Id = contact.Id,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Email = contact.Email,
                    CategoryId = contact.CategoryId,
                    Category = contact.Category != null ? new CategoryDto
                    {
                        Id = contact.Category.Id,
                        Name = contact.Category.Name
                    } : null,
                    SubcategoryId = contact.SubcategoryId,
                    Subcategory = contact.Subcategory != null ? new SubcategoryDto
                    {
                        Id = contact.Subcategory.Id,
                        Name = contact.Subcategory.Name,
                        CategoryId = contact.Subcategory.CategoryId
                    } : null,
                    Phone = contact.Phone,
                    DateOfBirth = contact.DateOfBirth
                };
                return Ok(contactDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching contact with ID: {Id}", id);
                return StatusCode(500, "An unexpected error occurred while fetching the contact.");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ContactDto>> CreateContact([FromBody] ContactDto contactDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdContact = await _contactService.CreateContactAsync(contactDto);
                var responseDto = new ContactDto
                {
                    Id = createdContact.Id,
                    FirstName = createdContact.FirstName,
                    LastName = createdContact.LastName,
                    Email = createdContact.Email,
                    CategoryId = createdContact.CategoryId,
                    Category = createdContact.Category != null ? new CategoryDto
                    {
                        Id = createdContact.Category.Id,
                        Name = createdContact.Category.Name
                    } : null,
                    SubcategoryId = createdContact.SubcategoryId,
                    Subcategory = createdContact.Subcategory != null ? new SubcategoryDto
                    {
                        Id = createdContact.Subcategory.Id,
                        Name = createdContact.Subcategory.Name,
                        CategoryId = createdContact.Subcategory.CategoryId
                    } : null,
                    Phone = createdContact.Phone,
                    DateOfBirth = createdContact.DateOfBirth
                };
                return CreatedAtAction(nameof(GetContact), new { id = createdContact.Id }, responseDto);
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
        public async Task<ActionResult<ContactDto>> UpdateContact(Guid id, [FromBody] ContactDto contactDto)
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
                var responseDto = new ContactDto
                {
                    Id = updatedContact.Id,
                    FirstName = updatedContact.FirstName,
                    LastName = updatedContact.LastName,
                    Email = updatedContact.Email,
                    CategoryId = updatedContact.CategoryId,
                    Category = updatedContact.Category != null ? new CategoryDto
                    {
                        Id = updatedContact.Category.Id,
                        Name = updatedContact.Category.Name
                    } : null,
                    SubcategoryId = updatedContact.SubcategoryId,
                    Subcategory = updatedContact.Subcategory != null ? new SubcategoryDto
                    {
                        Id = updatedContact.Subcategory.Id,
                        Name = updatedContact.Subcategory.Name,
                        CategoryId = updatedContact.Subcategory.CategoryId
                    } : null,
                    Phone = updatedContact.Phone,
                    DateOfBirth = updatedContact.DateOfBirth
                };
                return Ok(responseDto);
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