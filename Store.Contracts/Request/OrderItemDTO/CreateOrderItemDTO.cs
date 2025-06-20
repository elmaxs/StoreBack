namespace Store.Contracts.Request.OrderItemDTO
{
    public record CreateOrderItemDTO(
        Guid OrderId,
        Guid ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice
        );
}
