using Microsoft.EntityFrameworkCore;
using Store.Core.Abstractions.Repository;
using Store.Core.Models;
using Store.DataAccess.Entities;

namespace Store.DataAccess.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly OnlineStoreDbContext _context;

        public OrderItemRepository(OnlineStoreDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Create(Guid id, OrderItem orderItem)
        {
            var orderItemEntity = new OrderItemEntity
            {
                Id = id,
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId,
                Quantity = orderItem.Quantity,
                UnitPrice = orderItem.UnitPrice,
                TotalPrice = orderItem.TotalPrice
            };

            await _context.OrderItems.AddAsync(orderItemEntity);
            await _context.SaveChangesAsync();

            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.OrderItems.Where(oi => oi.Id == id).ExecuteDeleteAsync();

            return id;
        }

        public async Task<IEnumerable<OrderItem>>? GetAll()
        {
            var orderItemsEntity = await _context.OrderItems.Include(oi => oi.Order).Include(oi => oi.Product).ToListAsync();
            if (orderItemsEntity is null)
                return null;

            var result = orderItemsEntity.Select(oi => OrderItem.CreateOrderItem(oi.OrderId, oi.ProductId, oi.Product.Name,
                oi.Quantity, oi.UnitPrice).OrderItem);

            return result;
        }

        public async Task<OrderItem>? GetById(Guid id)
        {
            var orderItemEntity = await _context.OrderItems.Include(oi => oi.Order).Include(oi => oi.Product)
                .FirstOrDefaultAsync(oi => oi.Id == id);
            if (orderItemEntity is null) return null;

            var result = OrderItem.CreateOrderItem(orderItemEntity.OrderId, orderItemEntity.ProductId, orderItemEntity.Product.Name,
                orderItemEntity.Quantity, orderItemEntity.UnitPrice).OrderItem;

            return result;
        }

        public async Task<Guid> Update(Guid id, OrderItem orderItem)
        {
            await _context.OrderItems.Where(oi => oi.Id == id).ExecuteUpdateAsync(s => s
                .SetProperty(oi => oi.OrderId, oi => orderItem.OrderId)
                .SetProperty(oi => oi.ProductId, oi => orderItem.ProductId)
                .SetProperty(oi => oi.Quantity, oi => orderItem.Quantity)
                .SetProperty(oi => oi.UnitPrice, oi => orderItem.UnitPrice));

            return id;
        }
    }
}
