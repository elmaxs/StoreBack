using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Abstractions.User;
using Store.Contracts.UserContracts.Response.CategoryUserDTO;

namespace Store.API.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("get-subcategories-{id:guid}")]
        public async Task<ActionResult<ReadSubcategoriesDTO>> GetSubCategories(Guid id)
        {
            try
            {
                var result = await _categoryService.GetSubcategories(id);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("get-mains")]
        public async Task<ActionResult<IEnumerable<ReadMainCategories>>> GetMainsCategories()
        {
            try
            {
                var categories = await _categoryService.GetMainsCategories();

                return Ok(categories);
            }
            catch(Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("breadcrumb-{id:guid}")]
        public async Task<ActionResult<List<BreadcrumbCategoryDTO>>> GetBreadcrumbCategories(Guid id)
        {
            try
            {
                var breadcrumb = await _categoryService.BuildBreadcrumbAsync(id);

                return Ok(breadcrumb);
            }
            catch(Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
