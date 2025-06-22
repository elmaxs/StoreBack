using Store.Contracts.AdminContracts.Request.OrderDTO;
using Store.Contracts.AdminContracts.Response.OrderDTO;

namespace Store.Application.Abstractions.Admin
{
    public interface IAdminOrderService
    {
        Task<Guid> CreateOrder(AdminCreateOrderDTO orderDTO);
        Task<IEnumerable<AdminReadOrderDTO>> GetAllOrders();
        Task<AdminReadOrderDTO> GetOrderById(Guid id);
        Task<Guid> UpdateOrder(Guid id, AdminUpdateOrderDTO orderDTO);
        Task<Guid> DeleteOrder(Guid id);
    }
}
