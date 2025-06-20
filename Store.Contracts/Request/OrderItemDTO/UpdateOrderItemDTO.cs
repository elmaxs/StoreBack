namespace Store.Contracts.Request.OrderItemDTO
{
    public record UpdateOrderItemDTO(
        Guid ProductId,
        int Quantity,
        decimal UnitPrice
        );
}
