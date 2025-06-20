using Store.Core.Models;

namespace Store.Core.Abstractions.Repository
{
    public interface IOrderItemRepository
    {
        Task<Guid> Create(Guid id, OrderItem orderItem);
        Task<IEnumerable<OrderItem>>? GetAll();
        Task<OrderItem>? GetById(Guid id);
        Task<Guid> Update(Guid id, OrderItem orderItem);
        Task<Guid> Delete(Guid id);
    }
}
