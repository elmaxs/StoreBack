namespace Store.Contracts.Response.OrderDTO
{
    public record OrderItemInOrderDTO(
        Guid ProductId,
        int Quantity,
        decimal UnitPrice,
        decimal TotalPrice
        );
}
