namespace Store.DataAccess.Entities
{
    public class CategoryEntity
    {
        public Guid Id { get; set; }

        public Guid? ParentCategoryId { get; set; }
        public CategoryEntity? ParentCategory { get; set; }

        public string Name { get; set; } = null!;

        public ICollection<CategoryEntity> Subcategories { get; set; } = new List<CategoryEntity>();
        public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
    }
}
