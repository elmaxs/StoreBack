using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Abstractions.User;
using Store.Contracts.UserContracts.Request.ProductUserDTO;
using Store.Contracts.UserContracts.Response.ProductUserDTO;

namespace Store.API.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("products-byCategory")]
        public async Task<ActionResult<List<ReadProductByCategoryDTO>>> GetProductsByCategory([FromQuery] Guid categoryId,
            int page = 1, int pageSize = 4)
        {
            try
            {
                var products = await _productService.GetProductsByCategoryHierarchy(categoryId);

                return Ok(products);
            }
            catch(Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("products")]
        public async Task<ActionResult<List<ReadProductDTO>>> GetFiltered([FromQuery] ProductFilterParams filters)
        {
            if (!ModelState.IsValid)
                return BadRequest("Bad data");

            try
            {
                var product = await _productService.GetFilteredProductsAsync(filters);

                return Ok(product);
            }
            catch(Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
