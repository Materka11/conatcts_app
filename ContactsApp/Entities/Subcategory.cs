using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactsApp.Entities
{
    public class Subcategory
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Subcategory name is required.")]
        [StringLength(100, ErrorMessage = "Subcategory name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category ID is required.")]
        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public List<Contact> Contacts { get; set; } = new List<Contact>();
    }
}