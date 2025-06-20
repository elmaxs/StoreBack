namespace Store.Contracts.Response.OrderItemDTO
{
    public record ReadOrderItemDTO(
        Guid OrderId,
        Guid ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice
        );
}
