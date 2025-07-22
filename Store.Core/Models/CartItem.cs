namespace Store.Core.Models
{
    public class CartItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }

        public int Quantity { get; set; }
        public int AvailableQuantity { get; set; }
        public decimal UnitPrice { get; set; }

        public string ImageUrl { get; set; }

        public decimal TotalPrice => Quantity * UnitPrice;

        private CartItem(Guid productId, string productName, int quantity, int availableQuantity,
            decimal unitPrice, string imageUrl)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            ImageUrl = imageUrl;
        }

        public static (CartItem CartItem, string Error) CreateCartItem(Guid productId, string productName, 
            int quantity, int availableQuantity, decimal unitPrice, string imageUrl)
        {
            string error = string.Empty;

            if (productId == Guid.Empty || productId == new Guid())
            {
                error = "Id cant be empty or default value";
            }
            if (string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(imageUrl))
                error = "Product name and image url cant be null or empty";
            if (quantity < 1)
                error = "Quantity in cant be less than 1";
            if (unitPrice < 0)
                error = "Unit price cant be less than 0";

            var cartItem = new CartItem(productId, productName, quantity, availableQuantity, unitPrice, imageUrl);

            return (cartItem, error);
        }
    }
}
