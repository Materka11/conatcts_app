using ContactsApp.Data;
using ContactsApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Services.SubcategoryService
{
    public class SubcategoryService : ISubcategoryService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public SubcategoryService(AppDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<List<Subcategory>> GetAllSubcategoriesAsync()
        {
            return await _context.Subcategories
                 .Include(c => c.Category)
                 .Include(c => c.Contacts)
                 .ToListAsync();
        }
    }
}
