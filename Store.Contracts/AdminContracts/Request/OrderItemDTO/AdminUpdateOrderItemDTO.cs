namespace Store.Contracts.AdminContracts.Request.OrderItemDTO
{
    public record AdminUpdateOrderItemDTO(
        Guid ProductId,
        int Quantity,
        decimal UnitPrice
        );
}
