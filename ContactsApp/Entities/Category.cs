using System.ComponentModel.DataAnnotations;

namespace ContactsApp.Entities
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public List<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
        public List<Contact> Contacts { get; set; } = new List<Contact>();
    }
}