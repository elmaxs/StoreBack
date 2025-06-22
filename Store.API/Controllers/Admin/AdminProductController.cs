using Microsoft.AspNetCore.Mvc;
using Store.Application.Abstractions.Admin;
using Store.Contracts.AdminContracts.Request.ProductDTO;
using Store.Contracts.AdminContracts.Response.ProductDTO;
using Store.Core.Exceptions;

namespace Store.API.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AdminProductController : ControllerBase
    {
        //Example
        // GET /products?categoryId=3&page=1&pageSize=20
        //[HttpGet]
        //public async Task<IActionResult> GetProducts([FromQuery] Guid categoryId, int page = 1, int pageSize = 20)
        private readonly IAdminProductService _productService;

        public AdminProductController(IAdminProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("products")]
        public async Task<ActionResult<List<AdminReadProductDTO>>> GetAllProductsByAmin()
        {
            try
            {
                var result = await _productService.GetAllProducts();

                return Ok(result);
            }
            catch(Exception ex)
            {
                return NotFound(new { Message = ex});
            }
        }

        [HttpGet("product")]
        public async Task<ActionResult<AdminReadProductDTO>> GetProductByIdByAdmin([FromQuery] Guid productId)
        {
            try
            {
                var result = await _productService.GetProductById(productId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex });
            }
        }

        //[HttpGet("products/by-category")]
        //public async Task<ActionResult<AdminReadProductDTO>> GetProductsByCategoryId([FromQuery] Guid categoryId)
        //{
        //    try
        //    {
        //        var result = await _productService.GetProductsByCategoryId(categoryId);

        //        return Ok(result);
        //    }
        //    catch(Exception ex)
        //    {
        //        return NotFound(new { Message = ex });
        //    }
        //}

        [HttpPost("create")]
        public async Task<ActionResult<Guid>> CreateProduct([FromBody] AdminCreateProductDTO productDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessages.BadDataDTO);

            try
            {
                var result = await _productService.CreateProduct(productDTO);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new { error = ex });
            }
        }

        [HttpPut("update-{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateProduct(Guid id, [FromBody] AdminUpdateProductDTO productDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessages.BadDataDTO);

            try
            {
                var result = await _productService.UpdateProduct(id, productDTO);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message = ex });
            }
        }

        [HttpDelete("delete-{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteProductById(Guid id)
        {
            try
            {
                var result = await _productService.DeleteProduct(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex });
            }
        }
    }
}
