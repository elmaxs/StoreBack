using Store.Core.Models;

namespace Store.Core.Abstractions.Repository
{
    public interface IOrderRepository
    {
        Task<Guid> Create(Order order);
        Task<IEnumerable<Order>>? GetAll();
        Task<Order>? GetById(Guid id);
        Task<Guid> Update(Guid id, Order order);
        Task<Guid> Delete(Guid id);
    }
}
