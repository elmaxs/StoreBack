using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Abstractions;
using Store.Contracts.Request.OrderDTO;
using Store.Contracts.Response.OrderDTO;
using Store.Core.Exceptions;

namespace Store.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<List<ReadOrderDTO>>> GetAllOrders()
        {
            try
            {
                var result = await _orderService.GetAllOrders();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex });
            }
        }

        [HttpGet("get-{id:guid}")]
        public async Task<ActionResult<ReadOrderDTO>> GetOrderById(Guid id)
        {
            try
            {
                var result = await _orderService.GetOrderById(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex });
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Guid>> CreateOrder([FromBody] CreateOrderDTO orderItemDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessages.BadDataDTO);

            try
            {
                var result = await _orderService.CreateOrder(orderItemDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex });
            }
        }

        [HttpPut("update-{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateOrder(Guid id, [FromBody] UpdateOrderDTO orderItemDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorMessages.BadDataDTO);

            try
            {
                var result = await _orderService.UpdateOrder(id, orderItemDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex });
            }
        }

        [HttpDelete("delete-{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteOrderById(Guid id)
        {
            try
            {
                var result = await _orderService.DeleteOrder(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex });
            }
        }
    }
}
