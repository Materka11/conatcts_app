using ContactsApp.Entities;

namespace ContactsApp.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync();
    }
}
