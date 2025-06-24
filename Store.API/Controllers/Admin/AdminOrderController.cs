using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Abstractions.Admin;
using Store.Contracts.AdminContracts.Request.OrderDTO;
using Store.Contracts.AdminContracts.Response.OrderDTO;
using Store.Core.Exceptions;

namespace Store.API.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AdminOrderController : ControllerBase
    {
        private readonly IAdminOrderService _orderService;

        public AdminOrderController(IAdminOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<List<AdminReadOrderDTO>>> GetAllOrders()
        {
            try
            {
                var result = await _orderService.GetAllOrders();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("get-{id:guid}")]
        public async Task<ActionResult<AdminReadOrderDTO>> GetOrderById(Guid id)
        {
            try
            {
                var result = await _orderService.GetOrderById(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Guid>> CreateOrder([FromBody] AdminCreateOrderDTO orderItemDTO)
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
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("update-{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateOrder(Guid id, [FromBody] AdminUpdateOrderDTO orderItemDTO)
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
                return BadRequest(new { message = ex.Message });
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
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
