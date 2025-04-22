using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactsApp.Entities
{
    public class Contact
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [ForeignKey("Subcategory")]
        public Guid? SubcategoryId { get; set; }
        public Subcategory? Subcategory { get; set; }

        [StringLength(20, ErrorMessage = "Phone cannot exceed 50 characters.")]
        public string? Phone { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}
