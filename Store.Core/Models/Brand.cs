namespace Store.Core.Models
{
    public class Brand
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<Product> Products { get; set; } = new List<Product>();

        private Brand(Guid id, string name, string description, IEnumerable<Product> products)
        {
            Id = id;
            Name = name;
            Description = description;
            Products = products;
        }

        public static (Brand Brand, string Error) CreateBrand(Guid id, string name, string description, List<Product> products)
        {
            string error = string.Empty;

            if (id == Guid.Empty || id == new Guid())
            {
                error = "Id cant be empty or default value";
            }
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description))
            {
                error = "Brand name or description cant be null or empty";
            }

            var brand = new Brand(id, name, description, products);

            return (brand, error);
        }
    }
}
