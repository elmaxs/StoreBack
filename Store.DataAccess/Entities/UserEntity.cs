using Store.Core.Enums;

namespace Store.DataAccess.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }

        public string? FullName { get; set; }
        public string Username { get; set; } = null!;
        public string HashedPassword { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }

        public UserRole Role { get; set; } = UserRole.Customer;
        public DateTime CreatedAt { get; set; }

        public ICollection<ProductReviewEntity> Reviews { get; set; } = new List<ProductReviewEntity>();
        public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
    }
}
