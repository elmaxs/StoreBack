using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Abstractions.User;
using Store.Contracts.UserContracts.Request.ProductReviewDTO;
using Store.Contracts.UserContracts.Response.ProductReviewDTO;

namespace Store.API.Controllers.User
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductReviewController : ControllerBase
    {
        private readonly IProductReviewService _productReviewService;

        public ProductReviewController(IProductReviewService productReviewService)
        {
            _productReviewService = productReviewService;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<ICollection<ReadProductReviewDTO>>> GetAllReviews()
        {
            try
            {
                var result = await _productReviewService.GetAllProductReviews();

                return Ok(result);
            }
            catch(Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("get/{id:guid}")]
        public async Task<ActionResult<ReadProductReviewDTO>> GetReviewById(Guid id)
        {
            try
            {
                var result = await _productReviewService.GetProductReviewById(id);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("create")]
        public async Task<ActionResult<Guid>> CreateProductReview([FromBody] CreateProductReviewDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Bad data");

            try
            {
                var result = await _productReviewService.CreateProductReview(request);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("delete/{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteReview(Guid id)
        {
            try
            {
                var result = await _productReviewService.DeleteProductReview(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("update/{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateReview([FromQuery] Guid id, [FromBody] UpdateProductReviewDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Bad data");

            try
            {
                var result = await _productReviewService.UpdateProductReview(id, request);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
