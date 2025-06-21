namespace Store.Contracts.AdminContracts.Request.OrderItemDTO
{
    public record AdminCreateOrderItemDTO(
        Guid OrderId,
        Guid ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice
        );
}
