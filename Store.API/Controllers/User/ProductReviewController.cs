using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Abstractions.User;
using Store.Contracts.UserContracts.Request.ProductReviewDTO;
using Store.Contracts.UserContracts.Response.ProductReviewDTO;

namespace Store.API.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductReviewController : ControllerBase
    {
        private readonly IProductReviewService _productReviewService;

        public ProductReviewController(IProductReviewService productReviewService)
        {
            _productReviewService = productReviewService;
        }

        [HttpGet("product")]
        public async Task<ActionResult<ICollection<ReadProductReviewDTO>>> GetReviewsForProduct([FromQuery] Guid productId)
        {
            try
            {
                var result = await _productReviewService.GetReviewsForProduct(productId);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("ratings/info")]
        public async Task<ActionResult<Dictionary<int,int>>> GetRatingForProduct([FromQuery] Guid productId)
        {
            try
            {
                var result = await _productReviewService.GetRatingsInfoForProduct(productId);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("average")]
        public async Task<ActionResult<double>> GetAverageRatingForProduct([FromQuery] Guid productId)
        {
            try
            {
                var result = await _productReviewService.GetAverageRatingForProduct(productId);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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

        [HttpGet("ratings")]
        public async Task<ActionResult<ReadRatingsDTO>> GetRatings([FromQuery] Guid productId)
        {
            try
            {
                var result = await _productReviewService.GetRatingsDTO(productId);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
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

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<Guid>> CreateProductReview([FromBody] CreateProductReviewDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Bad data");

            try
            {
                var userId = User.FindFirst("userId")?.Value;
                var result = await _productReviewService.CreateProductReview(Guid.Parse(userId), request);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("delete/{id:guid}")]
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

        [Authorize]
        [HttpPut("update/{id:guid}")]
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
