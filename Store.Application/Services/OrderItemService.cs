using Store.Application.Abstractions;
using Store.Contracts.AdminContracts.Request.OrderItemDTO;
using Store.Contracts.AdminContracts.Response.OrderItemDTO;
using Store.Core.Abstractions.Repository;
using Store.Core.Exceptions;
using Store.Core.Models;

namespace Store.Application.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _itemRepository;

        public OrderItemService(IOrderItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<Guid> CreateOrderItem(AdminCreateOrderItemDTO orderItemDTO)
        {
            if (orderItemDTO is null)
                throw new ValidationException(ErrorMessages.BadDataDTO);

            var id = Guid.NewGuid();
            var (orderItem, error) = OrderItem.CreateOrderItem(orderItemDTO.OrderId, orderItemDTO.ProductId, orderItemDTO.ProductName,
                orderItemDTO.Quantity, orderItemDTO.UnitPrice);

            if (!string.IsNullOrEmpty(error))
                throw new ErrorDuringCreation(error);

            return await _itemRepository.Create(id, orderItem);
        }

        public async Task<Guid> DeleteOrderItem(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            return await _itemRepository.Delete(id);
        }

        public async Task<IEnumerable<AdminReadOrderItemDTO>> GetAllOrderItem()
        {
            var orderItems = await _itemRepository.GetAll();
            if (orderItems is null)
                throw new NotFound(ErrorMessages.OrderItemNotFound);

            return orderItems.Select(oi => new AdminReadOrderItemDTO
                (oi.OrderId, oi.ProductId, oi.ProductName, oi.Quantity, oi.UnitPrice)).ToList();
        }

        public async Task<AdminReadOrderItemDTO> GetOrderItemById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            var orderItem = await _itemRepository.GetById(id);
            if (orderItem is null)
                throw new NotFound(ErrorMessages.OrderItemNotFound);

            return new AdminReadOrderItemDTO(orderItem.OrderId, orderItem.ProductId, orderItem.ProductName,
                orderItem.Quantity, orderItem.UnitPrice);
        }

        public async Task<Guid> UpdateOrderItem(Guid id, AdminUpdateOrderItemDTO orderItemDTO)
        {
            if (id == Guid.Empty)
                throw new ValidationException(ErrorMessages.GuidCannotBeEmpty);

            var orderItem = await _itemRepository.GetById(id);
            if (orderItem is null)
                throw new NotFound(ErrorMessages.OrderItemNotFound);

            orderItem.ProductId = orderItemDTO.ProductId;
            orderItem.Quantity = orderItemDTO.Quantity;
            orderItem.UnitPrice = orderItemDTO.UnitPrice;

            return await _itemRepository.Update(id, orderItem);
        }
    }
}
