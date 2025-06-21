using Store.Application.Abstractions;
using Store.Contracts.AdminContracts.Request.OrderDTO;
using Store.Contracts.AdminContracts.Response.OrderDTO;
using Store.Core.Abstractions.Repository;
using Store.Core.Enums;
using Store.Core.Exceptions;
using Store.Core.Models;

namespace Store.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Guid> CreateOrder(AdminCreateOrderDTO orderDTO)
        {
            if (orderDTO is null)
                throw new BadDataDTO(ErrorMessages.BadDataDTO);

            var status = ParseOrderStatus(orderDTO.Status);

            var id = Guid.NewGuid();
            var (order, error) = Order.CreateOrder(id, orderDTO.UserId, orderDTO.CreatedAt, orderDTO.TotalPrice, 
                status, new List<OrderItem>());

            if (!string.IsNullOrEmpty(error))
                throw new ErrorDuringCreation(error);

            return await _orderRepository.Create(order);
        }

        public async Task<Guid> DeleteOrder(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            return await _orderRepository.Delete(id);
        }

        public async Task<IEnumerable<AdminReadOrderDTO>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAll();
            if (orders is null)
                throw new NotFound(ErrorMessages.OrderNotFound);

            var ordersDTO = orders.Select(o =>
            {
                var orderItemDTO = o.Items.Select(oi => new AdminOrderItemInOrderDTO(
                    oi.ProductId,
                    oi.Quantity,
                    oi.UnitPrice,
                    oi.TotalPrice
                    )).ToList();

                var orderDTO = new AdminReadOrderDTO(o.Id, o.UserId, o.CreatedAt, o.TotalPrice, (int)o.Status, orderItemDTO);

                return orderDTO;
            }).ToList();

            return ordersDTO;
        }

        public async Task<AdminReadOrderDTO> GetOrderById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            var order = await _orderRepository.GetById(id);
            if (order is null)
                throw new NotFound(ErrorMessages.OrderNotFound);

            var orderItemDTO = order.Items.Select(oi => new AdminOrderItemInOrderDTO(
                    oi.ProductId,
                    oi.Quantity,
                    oi.UnitPrice,
                    oi.TotalPrice
                    )).ToList();

            return new AdminReadOrderDTO(order.Id, order.UserId, order.CreatedAt, order.TotalPrice, (int)order.Status, orderItemDTO);
        }

        public async Task<Guid> UpdateOrder(Guid id, AdminUpdateOrderDTO orderDTO)
        {
            if (id == Guid.Empty)
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            if (orderDTO is null)
                throw new BadDataDTO(ErrorMessages.BadDataDTO);

            var order = await _orderRepository.GetById(id);
            if (order is null)
                throw new NotFound(ErrorMessages.OrderNotFound);

            order.CreatedAt = order.CreatedAt;
            order.TotalPrice = order.TotalPrice;
            order.Status = ParseOrderStatus(orderDTO.Status);

            return await _orderRepository.Update(id, order);
        }

        private OrderStatus ParseOrderStatus(int statusValue)
        {
            if (!Enum.IsDefined(typeof(OrderStatus), statusValue))
                throw new BadDataDTO("Недопустимый статус заказа.");

            return (OrderStatus)statusValue;
        }
    }
}
