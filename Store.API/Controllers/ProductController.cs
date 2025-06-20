using Microsoft.AspNetCore.Mvc;
using Store.Application.Abstractions;
using Store.Contracts.Request.ProductDTO;
using Store.Contracts.Response.ProductDTO;
using Store.Core.Exceptions;

namespace Store.API.Controllers
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

        [HttpGet("get-all")]
        public async Task<ActionResult<List<ReadProductDTO>>> GetAllProducts()
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

        [HttpGet("get-{id:guid}")]
        public async Task<ActionResult<ReadProductDTO>> GetProductById(Guid id)
        {
            try
            {
                var result = await _productService.GetProductById(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex });
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Guid>> CreateProduct([FromBody] CreateProductDTO productDTO)
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
        public async Task<ActionResult<Guid>> UpdateProduct(Guid id, [FromBody] UpdateProductDTO productDTO)
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
