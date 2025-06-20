using Store.Core.Enums;

namespace Store.Core.Models
{
    public class Order
    {
        public Guid Id { get; }
        public Guid UserId { get; }

        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; }

        public List<OrderItem> Items { get; set; }

        private Order(Guid id, Guid userId, DateTime createdAt, decimal totalPrice, OrderStatus status, List<OrderItem> items)
        {
            Id = id;
            UserId = userId;
            CreatedAt = createdAt;
            TotalPrice = totalPrice;
            Status = status;
            Items = items;
        }

        public static (Order Order, string Error) CreateOrder(Guid id, Guid userId, DateTime createdAt, decimal totalPrice,
            OrderStatus status, List<OrderItem> items)
        {
            string error = string.Empty;

            if (id == Guid.Empty || id == new Guid() || userId == Guid.Empty || userId == new Guid())
            {
                error = "Id cant be empty or default value";
            }
            if (totalPrice < 0)
                error = "Total price cant be less than 0";
            if (items == null || items.Count < 0)
                error = "Order items cant be empty or null";

            var order = new Order(id, userId, createdAt, totalPrice, status, items);

            return (order, error);
        }
    }
}
