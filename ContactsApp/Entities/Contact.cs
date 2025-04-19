﻿namespace ContactsApp.Entities
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        //hashowane haslo
        public string PasswordHash { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? Subcategory { get; set; }
        public string Phone { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
    }
}
