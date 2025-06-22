using Microsoft.AspNetCore.Mvc;
using Store.Application.Abstractions.Admin;
using Store.Contracts.AdminContracts.Request.CategoryDTO;
using Store.Contracts.AdminContracts.Response.CategoryDTO;

namespace Store.API.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AdminCategoryController : ControllerBase
    {
        private readonly IAdminCategoryService _categoryService;

        public AdminCategoryController(IAdminCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<List<AdminReadCategoryDTO>>> GetAllCategories()
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
        public async Task<ActionResult<AdminReadCategoryDTO>> GetCategoryById(Guid id)
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
        public async Task<ActionResult<Guid>> CreateCategory([FromBody] AdminCreateCategoryDTO categoryDTO)
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
        public async Task<ActionResult<Guid>> UpdateCategory(Guid id, AdminUpdateCategoryDTO categoryDTO)
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
