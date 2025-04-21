using ContactsApp.Entities;

namespace ContactsApp.Services.SubcategoryService
{
    public interface ISubcategoryService
    {
        Task<List<Subcategory>> GetAllSubcategoriesAsync();
    }
}
