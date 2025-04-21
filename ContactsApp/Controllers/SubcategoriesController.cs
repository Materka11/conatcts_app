using ContactsApp.Models;
using ContactsApp.Services.SubcategoryService;
using Microsoft.AspNetCore.Mvc;

namespace ContactsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoriesController : ControllerBase
    {
        private readonly ISubcategoryService _subcategoryService;

        public SubcategoriesController(ISubcategoryService subcategoryService)
        {
            _subcategoryService = subcategoryService ?? throw new ArgumentNullException(nameof(subcategoryService));
        }

        // Pobiera wszystkie podkategorie
        [HttpGet]
        public async Task<ActionResult<List<SubcategoryDto>>> GetAllSubcategories()
        {
            try
            {
                var subcategories = await _subcategoryService.GetAllSubcategoriesAsync();
                return Ok(subcategories);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}