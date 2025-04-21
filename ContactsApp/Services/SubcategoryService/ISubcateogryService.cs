using ContactsApp.Entities;

namespace ContactsApp.Services.SubcategoryService
{
    public interface ISubcateogryService
    {
        Task<List<Subcategory>> GetAllSubcategoriesAsync();
    }
}
