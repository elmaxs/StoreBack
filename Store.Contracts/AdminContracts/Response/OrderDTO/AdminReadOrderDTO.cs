namespace Store.Contracts.AdminContracts.Response.OrderDTO
{
    public record AdminReadOrderDTO(
        Guid Id,
        Guid UserId,
        DateTime CreatedAt,
        decimal TotalPrice,
        int Status,
        List<AdminOrderItemInOrderDTO> OrderItems
        );
}
