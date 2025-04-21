using ContactsApp.Entities;

namespace ContactsApp.Services.CateogryService
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync();
    }
}
