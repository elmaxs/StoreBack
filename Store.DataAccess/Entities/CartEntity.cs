using System.ComponentModel.DataAnnotations;

namespace Store.DataAccess.Entities
{
    public class CartEntity
    {
        [Key]
        public Guid UserId { get; set; }
        public UserEntity User { get; set; } = null!;

        public DateTime LastUpdated { get; set; }

        public ICollection<CartItemEntity> Items { get; set; } = new List<CartItemEntity>();
    }
}
