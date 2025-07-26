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

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ReadProductDTO>>> GetProductsBySearch([FromQuery] string search)
        {
            if (string.IsNullOrEmpty(search))
                return BadRequest("Enter some text");

            try
            {
                var result = await _productService.GetProductsBySearch(search);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("product/{id:guid}")]
        public async Task<ActionResult<ReadProductDTO>> GetProductById(Guid id)
        {
            try
            {
                var product = await _productService.GetProductById(id);

                return Ok(product);
            }
            catch(Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("products-byCategory")]
        public async Task<ActionResult<List<ReadProductDTO>>> GetProductsByCategory([FromQuery] Guid categoryId,
            int page = 1, int pageSize = 4)
        {
            try
            {
                var products = await _productService.GetProducts(categoryId);

                return Ok(products);
            }
            catch(Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("products/main")]
        public async Task<ActionResult<List<ReadProductBlockDTO>>> GetProductsForMainPage([FromQuery] ProductFilterParams filters)
        {
            if (!ModelState.IsValid)
                return BadRequest("Bad data");

            try
            {
                var products = await _productService.GetProductsForMainPage();
                return Ok(products);
            }
            catch(Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("products/pages")]
        public async Task<ActionResult<int>> GetCountPages([FromQuery] Guid categoryId)
        {
            try
            {
                return await _productService.GetCountPages(categoryId);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("products/filtered")]
        public async Task<ActionResult<List<ReadProductDTO>>> GetFiltered([FromQuery] ProductFilterParams filters)
        {
            if (!ModelState.IsValid)
                return BadRequest("Bad data");

            try
            {
                var products = await _productService.GetFilteredProductsAsync(filters);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}

