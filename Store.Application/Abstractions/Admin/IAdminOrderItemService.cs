using Store.Contracts.AdminContracts.Request.OrderItemDTO;
using Store.Contracts.AdminContracts.Response.OrderItemDTO;

namespace Store.Application.Abstractions.Admin
{
    public interface IAdminOrderItemService
    {
        Task<Guid> CreateOrderItem(AdminCreateOrderItemDTO orderItemDTO);
        Task<IEnumerable<AdminReadOrderItemDTO>> GetAllOrderItem();
        Task<AdminReadOrderItemDTO> GetOrderItemById(Guid id);
        Task<Guid> UpdateOrderItem(Guid id, AdminUpdateOrderItemDTO orderItemDTO);
        Task<Guid> DeleteOrderItem(Guid id);
    }
}
