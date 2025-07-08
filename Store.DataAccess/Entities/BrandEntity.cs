namespace Store.DataAccess.Entities
{
    public class BrandEntity
    {
        public Guid Id { get; set; }
        public string? Name { get; set; } = null;
        public string Description { get; set; } = null!;

        public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
    }
}
