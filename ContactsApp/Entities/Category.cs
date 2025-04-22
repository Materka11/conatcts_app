using System.ComponentModel.DataAnnotations;

namespace ContactsApp.Entities
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;
        public List<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
        public List<Contact> Contacts { get; set; } = new List<Contact>();
    }
}