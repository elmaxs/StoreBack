using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Abstractions.User;
using Store.Contracts.UserContracts.Request.CartUserDTO;
using Store.Contracts.UserContracts.Response.CartUserDTO;
using System.Security.Claims;

namespace Store.API.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add")]
        public async Task<ActionResult<Guid>> AddItemInCart([FromBody] CartItemDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Bad data");

            try
            {
                var userId = User.FindFirst("userId")?.Value;
                if (userId is null)     
                    return Unauthorized();
                
                var result = await _cartService.AddCartItem(Guid.Parse(userId), dto);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("cart")]
        public async Task<ActionResult<ReadCartDTO>> GetCart()
        {
            try
            {
                var userId = User.FindFirst("userId")?.Value;
                if (userId is null)
                    return Unauthorized();

                var result = await _cartService.GetCartByUserId(Guid.Parse(userId));

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<Guid>> UpdateCartItem([FromBody] CartItemDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Bad data");

            try
            {
                var userId = User.FindFirst("userId")?.Value;
                if (userId is null)
                    return Unauthorized();

                var result = await _cartService.UpdateQuantityProductInCart(Guid.Parse(userId), dto);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("clear")]
        public async Task<ActionResult<Guid>> ClearCart()
        {
            try
            {
                var userId = User.FindFirst("userId")?.Value;
                if (userId is null)
                    return Unauthorized();

                var result = await _cartService.ClearCart(Guid.Parse(userId));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("delete/item/{productId:guid}")]
        public async Task<ActionResult> DeleteCartItem(Guid productId)
        {
            try
            {
                var userId = User.FindFirst("userId")?.Value;
                if (userId is null)
                    return Unauthorized();

                var result = await _cartService.DeleteCartItem(Guid.Parse(userId), productId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
