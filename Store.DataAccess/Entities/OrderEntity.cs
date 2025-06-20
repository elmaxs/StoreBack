using Store.Core.Enums;

namespace Store.DataAccess.Entities
{
    public class OrderEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public UserEntity User { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; }

        public ICollection<OrderItemEntity> Items { get; set; } = new List<OrderItemEntity>();
    }
}