namespace Store.Contracts.AdminContracts.Response.OrderItemDTO
{
    public record AdminReadOrderItemDTO(
        Guid OrderId,
        Guid ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice
        );
}
