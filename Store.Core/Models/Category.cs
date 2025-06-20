

namespace Store.Core.Models
{
    public class Category
    {
        public Guid Id { get; }

        public string CategoryName { get; set; }
        public Guid? ParentCategoryId { get; set; }

        public int ProductCount { get; set; }

        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IEnumerable<Category> Subcategories { get; set; } = new List<Category>();

        private Category(Guid id, string categoryName, Guid? parentCategoryId, int productCount, 
            List<Product> products, List<Category> subcategory)
        {
            Id = id;
            CategoryName = categoryName;
            ParentCategoryId = parentCategoryId;
            ProductCount = productCount;
            Products = products;
            Subcategories = subcategory;
        }

        public static (Category Category, string Error) CreateCategory(Guid id, string categoryName, Guid? parentCategoryId, 
            int productCount, List<Product> products, List<Category> subcategory)
        {
            string error = string.Empty;

            if (id == Guid.Empty || id == new Guid())
                error = "Id cant be empty or default value";
            if (string.IsNullOrEmpty(categoryName))
                error = "Category name cant be null or empty";
            if (productCount < 0)
                error = "Product count cant be less than 0";

            var category = new Category(id, categoryName, parentCategoryId, productCount, products, subcategory);

            return (category, error);
        }
    }
}
