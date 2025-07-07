namespace Store.DataAccess.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; }

        public Guid CategoryId { get; set; }
        public CategoryEntity Category { get; set; } = null!;

        public Guid? BrandId { get; set; }
        public BrandEntity? Brand { get; set; } = null;

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;

        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public bool IsAvailable { get; set; }

        public ICollection<OrderItemEntity> OrderItems { get; set; } = new List<OrderItemEntity>();
    }
}