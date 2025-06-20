namespace Store.Core.Models
{
    public class OrderItem
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice => Quantity * UnitPrice;

        private OrderItem(Guid orderId, Guid productId, string productName, int quantity, decimal unitPrice)
        {
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public static (OrderItem OrderItem, string Error) CreateOrderItem(Guid orderId, Guid productId, string productName, 
            int quantity, decimal unitPrice)
        {
            string error = string.Empty;

            if (productId == Guid.Empty || productId == new Guid() || orderId == Guid.Empty || orderId == new Guid())
            {
                error = "Id cant be empty or default value";
            }
            if (string.IsNullOrEmpty(productName))
                error = "Product name cant be null or empty";
            if (quantity < 1)
                error = "Quantity in cant be less than 1";
            if (unitPrice < 0)
                error = "Unit price cant be less than 0";

            var orderItem = new OrderItem(orderId, productId, productName, quantity, unitPrice);

            return (orderItem, error);
        }
    }
}
