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
        private readonly ILogger<SubcategoriesController> _logger;

        public SubcategoriesController(ISubcategoryService subcategoryService, ILogger<SubcategoriesController> logger)
        {
            _subcategoryService = subcategoryService ?? throw new ArgumentNullException(nameof(subcategoryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<List<SubcategoryDto>>> GetAllSubcategories()
        {
            try
            {
                var subcategories = await _subcategoryService.GetAllSubcategoriesAsync();
                var subcategoriesDtos = subcategories.Select(s => new SubcategoryDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    CategoryId = s.CategoryId
                }).ToList();
                return Ok(subcategoriesDtos);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation while fetching subcategories.");
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching subcategories.");
                return StatusCode(500, "An unexpected error occurred while fetching subcategories.");
            }
        }
    }
}