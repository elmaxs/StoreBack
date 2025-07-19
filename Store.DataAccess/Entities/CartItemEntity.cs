namespace Store.DataAccess.Entities
{
    public class CartItemEntity
    {
        public Guid Id { get; set; }

        public CartEntity Cart { get; set; } = null!;
        public Guid CartId { get; set; }

        public ProductEntity Product { get; set; } = null!;
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
        public DateTime AddedAt { get; set; }
    }
}