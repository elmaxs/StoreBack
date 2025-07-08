

namespace Store.Core.Models
{
    public class Product
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }

        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }

        public Guid? BrandId { get; set; }
        public string? BrandName { get; set; }

        public int StockQuantity { get; set; }
        public bool IsAvailable => StockQuantity > 0;

        private Product(Guid id, string name, string description, string imageUrl, decimal price, 
            Guid categoryId, string categoryName, Guid? brandId, string? brandName, int stockQuantity)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            ImageUrl = imageUrl;
            CategoryId = categoryId;
            CategoryName = categoryName;
            BrandId = brandId;
            BrandName = brandName;
            StockQuantity = stockQuantity;
        }

        public static (Product Product, string Error) CreateProduct(Guid id, string name, string description, 
            string imageUrl, decimal price, Guid categoryId, string categoryName, Guid? brandId, string? brandName, int stockQuantity)
        {
            string error = string.Empty;

            if (id == Guid.Empty || id == new Guid())
            {
                error = "Id cant be empty or default value";
            }
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description) 
                || string.IsNullOrEmpty(imageUrl) || string.IsNullOrEmpty(categoryName)) //string.IsNullOrEmpty(brandName) ||
            {
                error = "Brand, product name, description, image url or category name cant be null or empty";
            }
            if (stockQuantity < 0)
                error = "Stock quantity cant be less than 0";

            var product = new Product(id, name, description, imageUrl, price, categoryId, categoryName, brandId, 
                brandName, stockQuantity);

            return (product, error);
        }
    }
}
