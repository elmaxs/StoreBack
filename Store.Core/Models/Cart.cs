using Store.Core.Exceptions;

namespace Store.Core.Models
{
    public class Cart
    {
        public Guid UserId { get; private set; }
        public DateTime LastUpdated { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

        private Cart(Guid userId, DateTime lastUpdated, ICollection<CartItem> items)
        {
            UserId = userId;
            LastUpdated = lastUpdated;
            Items = items;
        }

        public static (Cart? Cart, string? Error) CreateCart(Guid userId, DateTime lastUpdated, ICollection<CartItem> items)
        {
            if (userId == Guid.Empty)
                return (null, ErrorMessages.GuidCannotBeEmpty);

            var cart = new Cart(userId, lastUpdated, items);

            return (cart, null);
        }
    }
}
