using Store.Contracts.Request.OrderDTO;
using Store.Contracts.Response.OrderDTO;

namespace Store.Application.Abstractions
{
    public interface IOrderService
    {
        Task<Guid> CreateOrder(CreateOrderDTO orderDTO);
        Task<IEnumerable<ReadOrderDTO>> GetAllOrders();
        Task<ReadOrderDTO> GetOrderById(Guid id);
        Task<Guid> UpdateOrder(Guid id, UpdateOrderDTO orderDTO);
        Task<Guid> DeleteOrder(Guid id);
    }
}
