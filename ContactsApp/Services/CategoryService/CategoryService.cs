using ContactsApp.Data;
using ContactsApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public CategoryService(AppDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Include(c => c.Subcategories)
                .Include(c => c.Contacts)
                .ToListAsync();
        }
    }
}
