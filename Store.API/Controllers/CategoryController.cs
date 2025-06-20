using Microsoft.AspNetCore.Mvc;
using Store.Contracts.Request.CategoryDTO;
using Store.Contracts.Response.CategoryDTO;
using Store.Core.Abstractions.Services;

namespace Store.API.Controllers
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

        [HttpGet("get-all")]
        public async Task<ActionResult<List<ReadCategoryDTO>>> GetAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategory();

                return Ok(categories);
            }
            catch(Exception ex)
            {
                return NotFound(new { Message = ex });
            }
        }

        [HttpGet("get-{id:guid}")]
        public async Task<ActionResult<ReadCategoryDTO>> GetCategoryById(Guid id)
        {
            try
            {
                var category = await _categoryService.GetCategoryById(id);

                return Ok(category);
            }
            catch(Exception ex)
            {
                return NotFound(new { Message = ex });
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Guid>> CreateCategory([FromBody] CreateCategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("Bad data");

            try
            {
                var result = await _categoryService.CreateCategory(categoryDTO);

                return result;
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message = ex });
            }
        }

        [HttpPut("update-{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateCategory(Guid id, UpdateCategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("Bad data");

            try
            {
                var result = await _categoryService.UpdateCategory(id, categoryDTO);

                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex });
            }
        }

        [HttpDelete("delete-{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteCategory(Guid id)
        {
            try
            {
                var result = await _categoryService.DeleteCategory(id);

                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex });
            }
        }
    }
}
