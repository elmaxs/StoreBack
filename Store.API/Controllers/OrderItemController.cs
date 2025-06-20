using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Abstractions;
using Store.Contracts.Request.OrderItemDTO;
using Store.Contracts.Response.OrderItemDTO;
using Store.Core.Exceptions;

namespace Store.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<List<ReadOrderItemDTO>>> GetAllOrders()
        {
            try
            {
                var result = await _orderItemService.GetAllOrderItem();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex });
            }
        }

        [HttpGet("get-{id:guid}")]
        public async Task<ActionResult<ReadOrderItemDTO>> GetOrderItemById(Guid id)
        {
            try
            {
                var result = await _orderItemService.GetOrderItemById(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex });
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Guid>> CreateOrderItem([FromBody] CreateOrderItemDTO orderItemDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessages.BadDataDTO);

            try
            {
                var result = await _orderItemService.CreateOrderItem(orderItemDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex });
            }
        }

        [HttpPut("update-{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateOrderItem(Guid id, [FromBody] UpdateOrderItemDTO orderItemDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessages.BadDataDTO);

            try
            {
                var result = await _orderItemService.UpdateOrderItem(id, orderItemDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex });
            }
        }

        [HttpDelete("delete-{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteOrderItemById(Guid id)
        {
            try
            {
                var result = await _orderItemService.DeleteOrderItem(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex });
            }
        }
    }
}
