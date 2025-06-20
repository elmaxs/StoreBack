using Microsoft.EntityFrameworkCore;
using Store.Core.Abstractions.Repository;
using Store.Core.Models;
using Store.DataAccess.Entities;

namespace Store.DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OnlineStoreDbContext _context;

        public OrderRepository(OnlineStoreDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Create(Order order)
        {
            var orderEntity = new OrderEntity
            {
                Id = order.Id,
                UserId = order.UserId,
                CreatedAt = order.CreatedAt,
                TotalPrice = order.TotalPrice,
                Status = order.Status
            };

            await _context.Orders.AddAsync(orderEntity);
            await _context.SaveChangesAsync();

            return order.Id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.Orders.Where(o => o.Id == id).ExecuteDeleteAsync();

            return id;
        }

        public async Task<IEnumerable<Order>>? GetAll()
        {
            var ordersEntity = await _context.Orders.Include(o => o.Items).ThenInclude(oi => oi.Product).ToListAsync();
            if (ordersEntity is null)
                return null;

            var orders = ordersEntity.Select(o =>
            {
                var orderItems = o.Items.Select(oi =>
                    OrderItem.CreateOrderItem(oi.OrderId, oi.ProductId, oi.Product.Name, oi.Quantity, oi.UnitPrice).OrderItem).ToList();

                return Order.CreateOrder(o.Id, o.UserId, o.CreatedAt, o.TotalPrice, o.Status, orderItems).Order;
            }).ToList();

            return orders;
        }

        public async Task<Order>? GetById(Guid id)
        {
            var orderEntity = await _context.Orders.Include(o => o.Items).ThenInclude(oi => oi.Product).
                FirstOrDefaultAsync(o => o.Id == id);
            if (orderEntity is null)
                return null;

            var orderItems = orderEntity.Items.Select(oi => OrderItem.CreateOrderItem(oi.OrderId, oi.ProductId, oi.Product.Name,
                oi.Quantity, oi.UnitPrice).OrderItem).ToList();

            var order = Order.CreateOrder(orderEntity.Id, orderEntity.UserId, orderEntity.CreatedAt, orderEntity.TotalPrice,
                orderEntity.Status, orderItems).Order;

            return order;
        }

        public async Task<Guid> Update(Guid id, Order order)
        {
            await _context.Orders.Where(o => o.Id == id).ExecuteUpdateAsync(s => s
                .SetProperty(o => o.UserId, o => order.UserId)
                .SetProperty(o => o.CreatedAt, o => order.CreatedAt)
                .SetProperty(o => o.TotalPrice, o => order.TotalPrice)
                .SetProperty(o => o.Status, o => order.Status));

            return id;
        }
    }
}
