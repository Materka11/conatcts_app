namespace ContactsApp.Models
{
    public class SubcategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
    }
}
