namespace Store.Contracts.AdminContracts.Response.OrderDTO
{
    public record AdminOrderItemInOrderDTO(
        Guid ProductId,
        int Quantity,
        decimal UnitPrice,
        decimal TotalPrice
        );
}
