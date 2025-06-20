using Store.Contracts.Request.OrderItemDTO;
using Store.Contracts.Response.OrderItemDTO;

namespace Store.Application.Abstractions
{
    public interface IOrderItemService
    {
        Task<Guid> CreateOrderItem(CreateOrderItemDTO orderItemDTO);
        Task<IEnumerable<ReadOrderItemDTO>> GetAllOrderItem();
        Task<ReadOrderItemDTO> GetOrderItemById(Guid id);
        Task<Guid> UpdateOrderItem(Guid id, UpdateOrderItemDTO orderItemDTO);
        Task<Guid> DeleteOrderItem(Guid id);
    }
}
